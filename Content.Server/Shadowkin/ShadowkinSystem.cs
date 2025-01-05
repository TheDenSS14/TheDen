using Content.Shared.Examine;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Humanoid;
using Content.Shared.Psionics;
using Content.Shared.Bed.Sleep;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Shadowkin;
using Content.Shared.Rejuvenate;
using Content.Shared.Alert;
using Content.Shared.Rounding;
using Content.Shared.Actions;
using Robust.Shared.Prototypes;
using Content.Server.Abilities.Psionics;
using Content.Shared.Mobs; // Floofstation Edit
using Content.Server.FloofStation; // Floofstation Edit
using Content.Shared.Inventory; // Floofstation Edit
using Robust.Shared.Physics.Systems; // Floofstation Edit
using Robust.Shared.Random; // Floofstation Edit
using Robust.Shared.Utility; // Floofstation Edit
using System.Linq; // Floofstation Edit

namespace Content.Server.Shadowkin;

public sealed class ShadowkinSystem : EntitySystem
{
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilitiesSystem = default!;
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!; // Floofstation Edit
    [Dependency] private readonly InventorySystem _inventorySystem = default!; // Floofstation Edit
    [Dependency] private readonly IRobustRandom _random = default!; // Floofstation Edit
    [Dependency] private readonly EntityLookupSystem _lookup = default!; // Floofstation Edit
    [Dependency] private readonly SharedJointSystem _joints = default!; // Floofstation

    public const string ShadowkinSleepActionId = "ShadowkinActionSleep";

    private const int MaxRandomTeleportAttempts = 20; // Floofstation Edit
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ShadowkinComponent, ComponentStartup>(OnInit);
        SubscribeLocalEvent<ShadowkinComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<ShadowkinComponent, OnMindbreakEvent>(OnMindbreak);
        SubscribeLocalEvent<ShadowkinComponent, OnManaUpdateEvent>(OnManaUpdate);
        SubscribeLocalEvent<ShadowkinComponent, RejuvenateEvent>(OnRejuvenate);
        SubscribeLocalEvent<ShadowkinComponent, EyeColorInitEvent>(OnEyeColorChange);
        SubscribeLocalEvent<ShadowkinComponent, MobStateChangedEvent>(OnMobStateChanged); // Floofstation Edit
    }

    private void OnInit(EntityUid uid, ShadowkinComponent component, ComponentStartup args)
    {
        _actionsSystem.AddAction(uid, ref component.ShadowkinSleepAction, ShadowkinSleepActionId, uid);
    }

    private void OnEyeColorChange(EntityUid uid, ShadowkinComponent component, EyeColorInitEvent args)
    {
        if (!TryComp<HumanoidAppearanceComponent>(uid, out var humanoid)
            || humanoid.EyeColor == component.OldEyeColor)
            return;

        component.OldEyeColor = humanoid.EyeColor;
        humanoid.EyeColor = component.BlackEyeColor;
        Dirty(uid, humanoid);
    }

    private void OnExamined(EntityUid uid, ShadowkinComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange
            || !TryComp<PsionicComponent>(uid, out var magic)
            || HasComp<MindbrokenComponent>(uid))
            return;

        var severity = "shadowkin-power-" + ContentHelpers.RoundToLevels(magic.Mana, magic.MaxMana, 6);
        var powerType = Loc.GetString(severity);

        if (args.Examined == args.Examiner)
            args.PushMarkup(Loc.GetString("shadowkin-power-examined-self",
                ("power", Math.Floor(magic.Mana)),
                ("powerMax", Math.Floor(magic.MaxMana)),
                ("powerType", powerType)
            ));
        else
            args.PushMarkup(Loc.GetString("shadowkin-power-examined-other",
                ("target", uid),
                ("powerType", powerType)
            ));
    }

    private void OnManaUpdate(EntityUid uid, ShadowkinComponent component, ref OnManaUpdateEvent args)
    {
        if (!TryComp<PsionicComponent>(uid, out var magic))
            return;

        if (component.SleepManaRegen
            && TryComp<SleepingComponent>(uid, out var sleep))
            magic.ManaGainMultiplier = component.SleepManaRegenMultiplier;
        else
            magic.ManaGainMultiplier = 1;

        Dirty(uid, magic); // Update Shadowkin Overlay.
    }

    private void OnMindbreak(EntityUid uid, ShadowkinComponent component, ref OnMindbreakEvent args)
    {
        if (TryComp<MindbrokenComponent>(uid, out var mindbreak))
            mindbreak.MindbrokenExaminationText = "examine-mindbroken-shadowkin-message";

        if (TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
        {
            component.OldEyeColor = humanoid.EyeColor;
            humanoid.EyeColor = component.BlackEyeColor;
            Dirty(uid, humanoid);
        }

        if (TryComp<StaminaComponent>(uid, out var stamina))
            _stamina.TakeStaminaDamage(uid, stamina.CritThreshold, stamina, uid);
    }

    private void OnRejuvenate(EntityUid uid, ShadowkinComponent component, RejuvenateEvent args)
    {
        if (!HasComp<MindbrokenComponent>(uid))
            return;

        RemComp<MindbrokenComponent>(uid);

        if (TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
        {
            humanoid.EyeColor = component.OldEyeColor;
            Dirty(uid, humanoid);
        }

        EnsureComp<PsionicComponent>(uid, out var magic);
        magic.Mana = 200;
        magic.MaxMana = 200;
        magic.ManaGain = 0.25f;
        magic.MindbreakingFeedback = "shadowkin-blackeye";
        magic.NoMana = "shadowkin-tired";
    }

    // FloofStation Edit
    private void OnMobStateChanged(EntityUid uid, ShadowkinComponent component, MobStateChangedEvent args)
    {
        if (HasComp<MindbrokenComponent>(uid))
            return;

        if (args.NewMobState == MobState.Critical || args.NewMobState == MobState.Dead)
        {
            if (TryComp<InventoryComponent>(uid, out var inventoryComponent) && _inventorySystem.TryGetSlots(uid, out var slots))
                foreach (var slot in slots)
                    _inventorySystem.TryUnequip(uid, slot.Name, true, true, false, inventoryComponent);

            SpawnAtPosition("ShadowkinShadow", Transform(uid).Coordinates);
            SpawnAtPosition("EffectFlashShadowkinDarkSwapOff", Transform(uid).Coordinates);

            var query = EntityQueryEnumerator<DarkHubComponent>();
            while (query.MoveNext(out var target, out var portal))
            {
                var coords = Transform(target).Coordinates;
                var newCoords = coords.Offset(_random.NextVector2(5));
                for (var i = 0; i < MaxRandomTeleportAttempts; i++)
                {
                    var randVector = _random.NextVector2(5);
                    newCoords = coords.Offset(randVector);
                    if (!_lookup.GetEntitiesIntersecting(newCoords.ToMap(EntityManager, _transform), LookupFlags.Static).Any())
                        break;
                }

                _joints.RecursiveClearJoints(uid);

                _transform.SetCoordinates(uid, newCoords);
                continue;
            }

            SpawnAtPosition("ShadowkinShadow", Transform(uid).Coordinates);
            SpawnAtPosition("EffectFlashShadowkinDarkSwapOn", Transform(uid).Coordinates);

            RaiseLocalEvent(uid, new RejuvenateEvent());
            if (TryComp<PsionicComponent>(uid, out var magic))
            {
                magic.Mana = 0;
                EnsureComp<ForcedSleepingComponent>(uid);
            }
        }
    }
}
