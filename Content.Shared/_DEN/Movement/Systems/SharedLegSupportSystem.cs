using Content.Shared._DEN.Movement.Components;
using Content.Shared.Body.Components;
using Content.Shared.Hands;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;

namespace Content.Shared._DEN.Movement.Systems;

/// <summary>
///     A system that allows you to reduce the speed penalty incurred by losing legs, such as by
///     holding a "mobility aid" item in your hand.
/// </summary>
public abstract class SharedLegSupportSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _itemToggle = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifier = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<BodyComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMovementSpeedModifiers);

        SubscribeLocalEvent<HeldLegSupportComponent, GotEquippedHandEvent>(OnGotEquippedHand);
        SubscribeLocalEvent<HeldLegSupportComponent, GotUnequippedHandEvent>(OnGotUnequippedHand);
        SubscribeLocalEvent<HeldLegSupportComponent, ItemToggledEvent>(OnToggled);
        SubscribeLocalEvent<HeldLegSupportComponent, HeldRelayedEvent<ModifyLegLossSpeedPenaltyEvent>>
            (HeldModifySpeedPenalty);
    }

    private void OnGotEquippedHand(Entity<HeldLegSupportComponent> ent, ref GotEquippedHandEvent args)
    {
        _movementSpeedModifier.RefreshMovementSpeedModifiers(args.User);
    }

    private void OnGotUnequippedHand(Entity<HeldLegSupportComponent> ent, ref GotUnequippedHandEvent args)
    {
        _movementSpeedModifier.RefreshMovementSpeedModifiers(args.User);
    }

    private void OnToggled(EntityUid uid, LegSupportComponent comp, ref ItemToggledEvent args)
    {
        if (args.User != null)
            _movementSpeedModifier.RefreshMovementSpeedModifiers(args.User.Value);
    }

    private void OnRefreshMovementSpeedModifiers(Entity<BodyComponent> ent,
        ref RefreshMovementSpeedModifiersEvent args)
    {
        var body = ent.Comp;
        if (_standing.IsDown(ent.Owner)
            || body.RequiredLegs <= 0
            || body.LegEntities.Count >= body.RequiredLegs
            || !_itemToggle.IsActivated(ent.Owner))
            return;

        var legRatio = (float) body.LegEntities.Count / body.RequiredLegs;

        var (walkPenaltyReduction, sprintPenaltyReduction) = GetSpeedPenaltyReduction(ent);
        if (walkPenaltyReduction == 0f && sprintPenaltyReduction == 0f)
            return;

        // This calculation basically "partially increases" the number of legs the character has.
        // For example: If you're missing 1 out of 2 legs, you have a 50% leg ratio.
        // But if you have 50% walkspeed penalty reduction, you have a 75% leg ratio;
        // i.e. you effectively have 1.5 legs.
        var effectiveWalkLegRatio = legRatio + (1 - legRatio) * walkPenaltyReduction;
        var effectiveSprintLegRatio = legRatio + (1 - legRatio) * sprintPenaltyReduction;

        var walkMultiplier = effectiveWalkLegRatio / legRatio;
        var runMultiplier = effectiveSprintLegRatio / legRatio;

        args.ModifySpeed(walkMultiplier, runMultiplier);
    }

    public (float, float) GetSpeedPenaltyReduction(EntityUid uid)
    {
        var ev = new ModifyLegLossSpeedPenaltyEvent();
        RaiseLocalEvent(uid, ev);
        var walkPenaltyReduction = Math.Clamp(ev.WalkPenaltyReduction, 0.0f, 1.0f);
        var sprintPenaltyReduction = Math.Clamp(ev.SprintPenaltyReduction, 0.0f, 1.0f);

        return (walkPenaltyReduction, sprintPenaltyReduction);
    }

    private void HeldModifySpeedPenalty(Entity<HeldLegSupportComponent> ent,
        ref HeldRelayedEvent<ModifyLegLossSpeedPenaltyEvent> args)
        => ModifySpeedPenalty(ent.Comp, ref args.Args);

    private void ModifySpeedPenalty(LegSupportComponent comp, ref ModifyLegLossSpeedPenaltyEvent args)
        => args.ModifyPenalty(comp.WalkSpeedPenaltyReduction, comp.SprintSpeedPenaltyReduction);
}

public sealed class ModifyLegLossSpeedPenaltyEvent : EntityEventArgs
{
    public float WalkPenaltyReduction = 0f;
    public float SprintPenaltyReduction = 0f;

    public void ModifyPenalty(float walkPenaltyReduction, float sprintPenaltyReduction)
    {
        WalkPenaltyReduction = Math.Max(WalkPenaltyReduction, walkPenaltyReduction);
        SprintPenaltyReduction = Math.Max(SprintPenaltyReduction, sprintPenaltyReduction);
    }
}
