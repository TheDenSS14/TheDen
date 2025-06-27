using Content.Shared._DEN.Body;
using Content.Shared._DEN.Movement.Components;
using Content.Shared.Body.Components;
using Content.Shared.Hands;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Standing;

namespace Content.Shared._DEN.Movement.Systems;

public abstract class SharedSupportStandingSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _itemToggle = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<HeldSupportStandingComponent, GotUnequippedHandEvent>(OnGotUnequippedHand);
        SubscribeLocalEvent<WornSupportStandingComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<HeldSupportStandingComponent, ItemToggledEvent>(OnToggled);

        SubscribeLocalEvent<HeldSupportStandingComponent,
            HeldRelayedEvent<CannotSupportStandingEvent>>(SupportStandingWhenHeld);
        SubscribeLocalEvent<WornSupportStandingComponent,
            InventoryRelayedEvent<CannotSupportStandingEvent>>(SupportStandingWhenWorn);
    }

    private void OnGotUnequippedHand(Entity<HeldSupportStandingComponent> ent, ref GotUnequippedHandEvent args)
        => UpdateStanding(args.User);

    private void OnGotUnequipped(Entity<WornSupportStandingComponent> ent, ref GotUnequippedEvent args)
        => UpdateStanding(args.Equipee);

    private void OnToggled(EntityUid uid, SupportStandingComponent comp, ref ItemToggledEvent args)
    {
        if (args.User != null)
            UpdateStanding(args.User.Value);
    }

    protected void SupportStandingWhenHeld(Entity<HeldSupportStandingComponent> ent,
        ref HeldRelayedEvent<CannotSupportStandingEvent> args)
        => TrySupportStanding(ent.Owner, ent.Comp, ref args.Args);

    protected void SupportStandingWhenWorn(Entity<WornSupportStandingComponent> ent,
        ref InventoryRelayedEvent<CannotSupportStandingEvent> args)
        => TrySupportStanding(ent.Owner, ent.Comp, ref args.Args);

    private void UpdateStanding(EntityUid uid)
    {
        if (!TryComp<BodyComponent>(uid, out var body))
            return;

        var ev = new CannotSupportStandingEvent(body.LegEntities.Count);
        RaiseLocalEvent(uid, ev);

        if (!ev.Cancelled)
            _standing.Down(uid);
    }

    private void TrySupportStanding(EntityUid uid, SupportStandingComponent comp, ref CannotSupportStandingEvent args)
    {
        if (args.LegCount >= comp.MinimumLegCount && _itemToggle.IsActivated(uid))
            args.Cancel();
    }
}
