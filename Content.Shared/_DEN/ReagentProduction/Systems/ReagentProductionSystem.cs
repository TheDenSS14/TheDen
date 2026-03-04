// SPDX-FileCopyrightText: 2026 Alex C
//
// SPDX-License-Identifier: MIT

using System.Globalization;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Enumerable = System.Linq.Enumerable;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using static Content.Shared._DEN.ReagentProduction.Events.ReagentProductionEvents;
using Content.Shared._DEN.ReagentProduction.Components;
using Content.Shared._DEN.ReagentProduction.Events;
using Content.Shared._DEN.ReagentProduction.Prototypes;

namespace Content.Shared._DEN.ReagentProduction.Systems;

public sealed class ReagentProductionSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    private static readonly VerbCategory ReagentFillCategory = new("verb-categories-fill", "/Textures/_DEN/Interface/VerbIcons/lewd.svg.192dpi.png");

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ReagentProducerComponent, ReagentProductionTypeAdded>(ProductionTypeAdded);
        SubscribeLocalEvent<ReagentProducerComponent, ReagentProductionTypeRemoved>(ProductionTypeRemoved);

        SubscribeLocalEvent<RefillableSolutionComponent, GetVerbsEvent<InteractionVerb>>(AddVerbs);

        SubscribeLocalEvent<ReagentProducerComponent, ReagentProductionFillEvent>(FinishFillDoAfter);
        SubscribeLocalEvent<ReagentProducerComponent, MapInitEvent>(OnMapInit);

    }
    private void OnMapInit(Entity<ReagentProducerComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextUpdate = _gameTiming.CurTime + ent.Comp.UpdateInterval;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<ReagentProducerComponent, SolutionContainerManagerComponent>();
        while (query.MoveNext(out var uid, out var producerComponent, out _))
        {
            // If the mob is dead OR it isnt time for the next update, don't move foward
            if (!_mobState.IsAlive(uid) || _gameTiming.CurTime < producerComponent.NextUpdate)
                continue;

            producerComponent.NextUpdate += producerComponent.UpdateInterval;

            // for every production type the producer has
            foreach (var productionType in Enumerable.Select(producerComponent.ProductionTypes, productionTypeId => _protoManager.Index(productionTypeId)))
            {
                // ensure there's a solution to add to
                _solutionContainer.EnsureSolution(uid, productionType.SolutionName, out var solution, productionType.MaximumCapacity);

                if (solution == null)
                    continue;
                // do some math to figure out how much we can add
                var amountToAdd = FixedPoint2.Clamp(
                    solution.MaxVolume - solution.Volume,
                    FixedPoint2.Zero,
                    productionType.UnitsPerProduction);

                if (amountToAdd <= 0)
                    return;
                //and add it :)
                solution.AddReagent(productionType.Reagent, amountToAdd);
            }
        }
    }

    private void AddVerbs(Entity<RefillableSolutionComponent> container, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || !TryComp<ReagentProducerComponent>(args.User, out var producerComp))
            return;
        // Add a verb for every production type the producer has
        foreach (var productionTypeId in producerComp.ProductionTypes)
        {
            if (!_protoManager.TryIndex(productionTypeId, out var productionType) ||
                !_protoManager.TryIndex(productionType.Reagent, out var reagent))
                continue;

            var producer = args.User;
            var verb = new InteractionVerb
            {
                Category = ReagentFillCategory,
                Act = () => StartFillDoAfter((producer, producerComp), container, productionTypeId),
                Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reagent.LocalizedName),
                CloseMenu = true,
                Priority = -1
            };
            args.Verbs.Add(verb);
        }
    }

    private void StartFillDoAfter(
        Entity<ReagentProducerComponent> user,
        Entity<RefillableSolutionComponent> target,
        ProtoId<ReagentProductionTypePrototype> productionTypeId
    )
    {
        var productionType = _protoManager.Index(productionTypeId);

        _doAfter.TryStartDoAfter(
            new(EntityManager, user, productionType.FillTime, new ReagentProductionFillEvent(productionTypeId), user, target: target)
        {
            BreakOnMove = true,
            BreakOnDropItem = true
        });
    }

    private void FinishFillDoAfter(Entity<ReagentProducerComponent> ent, ref ReagentProductionFillEvent args)
    {
        if (!_protoManager.TryIndex(args.ProductionType, out var productionType) || args.Target == null || args.Cancelled)
            return;

        if (!TryComp<RefillableSolutionComponent>(args.Target.Value, out var refillableSolution))
            return;

        if (!_solutionContainer.TryGetSolution(ent.Owner, productionType.SolutionName, out var userSolutionComp)||
            !_solutionContainer.TryGetSolution(args.Target.Value, refillableSolution.Solution, out var targetSolutionComp))
            return;

        var targetSolution = targetSolutionComp.Value.Comp.Solution;

        // If theres no cum to cum you cant cum okay?
        if (userSolutionComp.Value.Comp.Solution.Volume <= 0)
        {
            _popup.PopupEntity(Loc.GetString(productionType.DryPopup),ent,ent);
            return;
        }

        // Get available volume in target solution
        var targetAvailableVolume = targetSolution.MaxVolume - targetSolution.Volume;

        // If theres no room just silently return
        if (targetAvailableVolume <= 0)
            return;

        // Get amount to add, attempts to add the largest amount with the maximum set from production type
        var amountToAdd =
            FixedPoint2.Clamp(targetAvailableVolume, FixedPoint2.Zero, productionType.MaximumLoad);

        var split = _solutionContainer.SplitSolution(userSolutionComp.Value, amountToAdd);

        var quantity = _solutionContainer.AddSolution(targetSolutionComp.Value, split);
        _popup.PopupEntity(
            Loc.GetString(
            productionType.SuccessPopup,
            ("amount", quantity),
            ("target", Identity.Entity(args.Target.Value, EntityManager))),
            args.Target.Value,
            args.Args.User,
            PopupType.Medium);
    }

    public void AddProductionType(EntityUid entity, ReagentProductionTypePrototype prototypeType)
    {
        EnsureComp<ReagentProducerComponent>(entity);

        RaiseLocalEvent(entity, new ReagentProductionTypeAdded(prototypeType));
    }

    public void RemoveProductionType(EntityUid entity, ReagentProductionTypePrototype prototypeType)
    {
        EnsureComp<ReagentProducerComponent>(entity);

        RaiseLocalEvent(entity, new ReagentProductionTypeRemoved(prototypeType));
    }
    private void ProductionTypeAdded(Entity<ReagentProducerComponent> ent, ref ReagentProductionTypeAdded args)
    {
        if (!_protoManager.TryIndex(args.ProductionType, out var productionType))
            return;

        ent.Comp.ProductionTypes.Add(args.ProductionType);

        _solutionContainer.EnsureSolution(ent.Owner, productionType.SolutionName,out _, out var solution, productionType.MaximumCapacity);
        solution?.AddReagent(productionType.Reagent, productionType.MaximumCapacity);
    }
    private void ProductionTypeRemoved(Entity<ReagentProducerComponent> ent, ref ReagentProductionTypeRemoved args)
    {
        ent.Comp.ProductionTypes.Remove(args.ProductionType);
        //If there are no more production types, just remove the component
        if (ent.Comp.ProductionTypes.Count == 0)
            RemCompDeferred<ReagentProducerComponent>(ent);
    }
}
