

using Content.Server.Store.Systems;
using Content.Shared._DEN.Skia;
using Content.Shared.Actions;
using Content.Shared.Bed.Sleep;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Humanoid;
using Content.Shared.Interaction;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Psionics.Glimmer;
using Content.Shared.Stealth.Components;
using Content.Shared.Store.Components;
using Content.Shared.Pinpointer;
using Content.Server.Objectives.Systems;
using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;
using Content.Shared.Inventory;

namespace Contnet.Server._DEN.Skia;

public sealed class SkiaSystem : SharedSkiaSystem
{

    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedInteractionSystem _interact = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StoreSystem _store = default!;
    [Dependency] private readonly MobThresholdSystem _mobThresholdSystem = default!;
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private readonly GlimmerSystem _glimmerSystem = default!;
    [Dependency] private readonly SharedPinpointerSystem _pinpointerSystem = default!;
    [Dependency] private readonly TargetObjectiveSystem _targetObjectiveSystem = default!;
    [Dependency] private readonly EntityLookupSystem _lookupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkiaComponent, MobStateChangedEvent>(OnMobStageChanged);
        SubscribeLocalEvent<SkiaComponent, SkiaShopActionEvent>(OnShop);
        SubscribeLocalEvent<SkiaComponent, ObjectiveAfterAssignEvent>(OnAfterAssign);
        SubscribeLocalEvent<SkiaComponent, UserActivateInWorldEvent>(OnInteract);
        SubscribeLocalEvent<SkiaComponent, SkiaReapingEvent>(OnReaping);
    }

    private void OnShop(EntityUid uid, SkiaComponent comp, SkiaShopActionEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        _store.ToggleUi(uid, uid, store);
    }

    private void OnMobStageChanged(EntityUid uid, SkiaComponent comp, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
            RemComp<StealthComponent>(uid);

        if (args.NewMobState == MobState.Alive && !TryComp<StealthComponent>(uid, out var stealthComp))
        {
            AddComp<StealthComponent>(uid);
        }
    }

    public void OnAfterAssign(EntityUid uid, SkiaComponent comp, ObjectiveAfterAssignEvent args)
    {
        if (!TryComp<TransformComponent>(uid, out var coords))
            return;

        // There has to be a better way to do this... Gets the Skias pinpointer hopefully...
        foreach (var pinpointer in _lookupSystem.GetEntitiesInRange<PinpointerComponent>(coords.Coordinates, 0))
        {
            _pinpointerSystem.SetTarget(pinpointer.Owner, args.MindId, pinpointer);
        }
    }

    private void OnInteract(EntityUid uid, SkiaComponent comp, UserActivateInWorldEvent args)
    {
        // Bail out if it's already happened or we're clicking ourselves
        if (args.Handled || args.Target == args.User)
            return;

        // If our target isn't a mob, doesn't look like a person, has been reaped before or doesn't have a mind, bail out
        if (!HasComp<MobStateComponent>(args.Target) || !HasComp<HumanoidAppearanceComponent>(args.Target) || HasComp<SoulReapedComponent>(args.Target) || !HasComp<MindComponent>(args.Target))
            return;

        BeginReapingDoAfter(uid, args.Target, comp);
        args.Handled = true;
    }

    private void BeginReapingDoAfter(EntityUid uid, EntityUid target, SkiaComponent comp)
    {
        // Same logic as the revenant, if they're dead or asleep you can harvest the target
        if (TryComp<MobStateComponent>(target, out var mobState) && mobState.CurrentState == MobState.Alive && !HasComp<SleepingComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("revenant-soul-too-powerful"), target, uid);
            return;
        }

        var doAfterArgs = new DoAfterArgs(EntityManager, uid, TimeSpan.FromSeconds(comp.ReapDuration), new SkiaReapingEvent(), uid, target: target)
        {
            DistanceThreshold = 2,
            BreakOnMove = true,
            BreakOnDamage = true,
            RequireCanInteract = false
        };

        if (!_doAfter.TryStartDoAfter(doAfterArgs))
            return;

        _popup.PopupEntity(Loc.GetString("revenant-soul-begin-harvest", ("target", target)),
            target, PopupType.Large);
    }

    private void OnReaping(EntityUid uid, SkiaComponent comp, SkiaReapingEvent args)
    {
        if (args.Cancelled || args.Target == null)
            return;

        if (!HasComp<MobStateComponent>(args.Target))
            return;


        // Get the exact amount of cold damage to kill them.
        if (!_mobThresholdSystem.TryGetThresholdForState(args.Target.Value, MobState.Dead, out var damage))
            return;

        _popup.PopupEntity(Loc.GetString("revenant-soul-finish-harvest", ("target", args.Target)),
            args.Target.Value, PopupType.LargeCaution);

        // Apply the cold damage
        DamageSpecifier damageSpec = new();
        damageSpec.DamageDict.Add("Cold", damage.Value);
        _damage.TryChangeDamage(args.Target, damageSpec, true, origin: uid);

        AddComp<SoulReapedComponent>(args.Target.Value);

        // Add soul silk to the Skia
        _store.TryAddCurrency(new Dictionary<string, FixedPoint2> { { comp.SoulSilkCurrencyPrototype, comp.SilkGained } }, uid);


        _glimmerSystem.DeltaGlimmerInput(comp.ReapGlimmerValue);

        args.Handled = true;
    }
}
