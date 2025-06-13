using Content.Server.Actions;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.NPC.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.Emag.Systems;
using Content.Shared.Mobs.Components;
using Content.Shared.Silicons.Bots;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Server.Silicons.Bots;

public sealed class MedibotSystem : SharedMedibotSystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly HypospraySystem _hypospray = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedibotComponent, ComponentStartup>(OnMedibotStartup);
        SubscribeLocalEvent<EmaggableMedibotComponent, GotEmaggedEvent>(OnEmagged);
        SubscribeLocalEvent<MedibotInjectTargetEvent>(OnInjectTarget);
    }

    private void OnMedibotStartup(EntityUid uid, MedibotComponent component, ComponentStartup args)
    {
        component.InjectorSlot = _container.EnsureContainer<ContainerSlot>(uid, component.SlotId);
        if (!EntityManager.TrySpawnInContainer(component.InjectorProto, uid, component.InjectorSlot.ID, out var _))
            Log.Error($"Failed to spawn injector {component.InjectorProto} for {ToPrettyString(uid)}!");

        _actions.AddAction(uid, ref component.InjectActionEntity, component.InjectActionId);
    }

    private void OnEmagged(EntityUid uid, EmaggableMedibotComponent comp, ref GotEmaggedEvent args)
    {
        if (!TryComp<MedibotComponent>(uid, out var medibot))
            return;

        _audio.PlayPredicted(comp.SparkSound, uid, args.UserUid);

        foreach (var (state, treatment) in comp.Replacements)
            medibot.Treatments[state] = treatment;

        args.Handled = true;
    }

    private void OnInjectTarget(ref MedibotInjectTargetEvent ev)
    {
        if (!ev.Handled && TryInjectTarget(ev.Performer, ev.Target))
            ev.Handled = true;
    }

    private bool TryInjectTarget(EntityUid uid, EntityUid target)
    {
        if (!TryComp<MedibotComponent>(uid, out var medibot)
            || HasComp<NPCRecentlyInjectedComponent>(target)
            || !TryComp<MobStateComponent>(target, out var state)
            || !TryGetTreatment(medibot, state.CurrentState, out var treatment))
            return false;

        var injectorId = medibot.InjectorSlot.ContainedEntity;
        if (injectorId == null
            || !TryComp<HyposprayComponent>(injectorId, out var hypospray)
            || !_solutionContainer.TryGetSolution(injectorId.Value, "injector", out var injectorSolution))
            return false;

        var hyposprayEnt = new Entity<HyposprayComponent>(injectorId.Value, hypospray);
        _solutionContainer.RemoveAllSolution(injectorSolution.Value);
        _solutionContainer.TryAddReagent(injectorSolution.Value, treatment.Reagent, treatment.Quantity, out _);
        _hypospray.TryDoInject(hyposprayEnt, target, uid, DuplicateConditions.SameEvent);
        return true;
    }
}
