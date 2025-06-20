// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Scribbles0 <91828755+Scribbles0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 Winkarst <74284083+Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Construction;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Ghost;
using Content.Server.Popups;
using Content.Server.Repairable;
using Content.Server.Stack;
using Content.Server.Wires;
using Content.Shared.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Database;
using Content.Shared.Destructible;
using Content.Shared.Emag.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Materials;
using Content.Shared.Mind;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Power;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Destructible;
using Content.Shared.Emag.Components;
using Content.Shared.Power;
using Robust.Shared.Prototypes;

namespace Content.Server.Materials;

/// <inheritdoc/>
public sealed class MaterialReclaimerSystem : SharedMaterialReclaimerSystem
{
    [Dependency] private readonly AppearanceSystem _appearance = default!;
    [Dependency] private readonly GhostSystem _ghostSystem = default!;
    [Dependency] private readonly MaterialStorageSystem _materialStorage = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedBodySystem _body = default!; //bobby
    [Dependency] private readonly PuddleSystem _puddle = default!;
    [Dependency] private readonly StackSystem _stack = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MaterialReclaimerComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<MaterialReclaimerComponent, RefreshPartsEvent>(OnRefreshParts);
        SubscribeLocalEvent<MaterialReclaimerComponent, UpgradeExamineEvent>(OnUpgradeExamine);
        SubscribeLocalEvent<MaterialReclaimerComponent, PowerChangedEvent>(OnPowerChanged);
        SubscribeLocalEvent<MaterialReclaimerComponent, InteractUsingEvent>(OnInteractUsing,
            before: new []{typeof(WiresSystem), typeof(SolutionTransferSystem)});
        SubscribeLocalEvent<MaterialReclaimerComponent, SuicideByEnvironmentEvent>(OnSuicideByEnvironment);
        SubscribeLocalEvent<ActiveMaterialReclaimerComponent, PowerChangedEvent>(OnActivePowerChanged);
    }
    private void OnStartup(Entity<MaterialReclaimerComponent> entity, ref ComponentStartup args)
    {
        _solutionContainer.EnsureSolution(entity.Owner, entity.Comp.SolutionContainerId);
    }

    private void OnUpgradeExamine(Entity<MaterialReclaimerComponent> entity, ref UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade(Loc.GetString("material-reclaimer-upgrade-process-rate"), entity.Comp.MaterialProcessRate / entity.Comp.BaseMaterialProcessRate);
    }

    private void OnRefreshParts(Entity<MaterialReclaimerComponent> entity, ref RefreshPartsEvent args)
    {
        var rating = args.PartRatings[entity.Comp.MachinePartProcessRate] - 1;
        entity.Comp.MaterialProcessRate = entity.Comp.BaseMaterialProcessRate * MathF.Pow(entity.Comp.PartRatingProcessRateMultiplier, rating);
        Dirty(entity);
    }

    private void OnPowerChanged(Entity<MaterialReclaimerComponent> entity, ref PowerChangedEvent args)
    {
        AmbientSound.SetAmbience(entity.Owner, entity.Comp.Enabled && args.Powered);
        entity.Comp.Powered = args.Powered;
        Dirty(entity);
    }

    private void OnInteractUsing(Entity<MaterialReclaimerComponent> entity, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        // if we're trying to get a solution out of the reclaimer, don't destroy it
        if (_solutionContainer.TryGetSolution(entity.Owner, entity.Comp.SolutionContainerId, out _, out var outputSolution) && outputSolution.Contents.Any())
        {
            if (TryComp<SolutionContainerManagerComponent>(args.Used, out var managerComponent) &&
                _solutionContainer.EnumerateSolutions((args.Used, managerComponent)).Any(s => s.Solution.Comp.Solution.AvailableVolume > 0))
            {
                if (_openable.IsClosed(args.Used))
                    return;

                if (TryComp<SolutionTransferComponent>(args.Used, out var transfer) &&
                    transfer.CanReceive)
                    return;
            }
        }

        args.Handled = TryStartProcessItem(entity.Owner, args.Used, entity.Comp, args.User);
    }

    private void OnSuicideByEnvironment(Entity<MaterialReclaimerComponent> entity, ref SuicideByEnvironmentEvent args)
    {
        if (args.Handled)
            return;

        var victim = args.Victim;
        if (TryComp(victim, out ActorComponent? actor) &&
            _mind.TryGetMind(actor.PlayerSession, out var mindId, out var mind))
        {
            _ghostSystem.OnGhostAttempt(mindId, false, mind: mind);
            if (mind.OwnedEntity is { Valid: true } suicider)
            {
                _popup.PopupEntity(Loc.GetString("recycler-component-suicide-message"), suicider);
            }
        }

        _popup.PopupEntity(Loc.GetString("recycler-component-suicide-message-others",
                ("victim", Identity.Entity(victim, EntityManager))),
            victim,
            Filter.PvsExcept(victim, entityManager: EntityManager),
            true);

        _body.GibBody(victim, true);
        _appearance.SetData(entity.Owner, RecyclerVisuals.Bloody, true);
        args.Handled = true;
    }

    private void OnActivePowerChanged(Entity<ActiveMaterialReclaimerComponent> entity, ref PowerChangedEvent args)
    {
        if (!args.Powered)
            TryFinishProcessItem(entity, null, entity.Comp);
    }

    /// <inheritdoc/>
    public override bool TryFinishProcessItem(EntityUid uid, MaterialReclaimerComponent? component = null, ActiveMaterialReclaimerComponent? active = null)
    {
        if (!Resolve(uid, ref component, ref active, false))
            return false;

        if (!base.TryFinishProcessItem(uid, component, active))
            return false;

        if (active.ReclaimingContainer.ContainedEntities.FirstOrNull() is not { } item)
            return false;

        Container.Remove(item, active.ReclaimingContainer);
        Dirty(uid, component);

        // scales the output if the process was interrupted.
        var completion = 1f - Math.Clamp((float) Math.Round((active.EndTime - Timing.CurTime) / active.Duration),
            0f, 1f);
        Reclaim(uid, item, completion, component);

        return true;
    }

    /// <inheritdoc/>
    public override void Reclaim(EntityUid uid,
        EntityUid item,
        float completion = 1f,
        MaterialReclaimerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        base.Reclaim(uid, item, completion, component);

        var xform = Transform(uid);

        SpawnMaterialsFromComposition(uid, item, completion * component.Efficiency, xform: xform);

        if (CanGib(uid, item, component))
        {
            _adminLogger.Add(LogType.Gib, LogImpact.Extreme, $"{ToPrettyString(item):victim} was gibbed by {ToPrettyString(uid):entity} ");
            SpawnChemicalsFromComposition(uid, item, completion, false, component, xform);
            _body.GibBody(item, true);
            _appearance.SetData(uid, RecyclerVisuals.Bloody, true);
        }
        else
        {
            SpawnChemicalsFromComposition(uid, item, completion, true, component, xform);
        }

        QueueDel(item);
    }

    private void SpawnMaterialsFromComposition(EntityUid reclaimer,
        EntityUid item,
        float efficiency,
        MaterialStorageComponent? storage = null,
        TransformComponent? xform = null,
        PhysicalCompositionComponent? composition = null)
    {
        if (!Resolve(reclaimer, ref storage, ref xform, false))
            return;

        if (!Resolve(item, ref composition, false))
            return;

        foreach (var (material, amount) in composition.MaterialComposition)
        {
            var outputAmount = (int) (amount * efficiency);
            _materialStorage.TryChangeMaterialAmount(reclaimer, material, outputAmount, storage);
        }

        foreach (var (storedMaterial, storedAmount) in storage.Storage)
        {
            var stacks = _materialStorage.SpawnMultipleFromMaterial(storedAmount, storedMaterial,
                xform.Coordinates,
                out var materialOverflow);
            var amountConsumed = storedAmount - materialOverflow;
            _materialStorage.TryChangeMaterialAmount(reclaimer, storedMaterial, -amountConsumed, storage);
            foreach (var stack in stacks)
            {
                _stack.TryMergeToContacts(stack);
            }
        }
    }

    private void SpawnChemicalsFromComposition(EntityUid reclaimer,
        EntityUid item,
        float efficiency,
        bool sound = true,
        MaterialReclaimerComponent? reclaimerComponent = null,
        TransformComponent? xform = null,
        PhysicalCompositionComponent? composition = null)
    {
        if (!Resolve(reclaimer, ref reclaimerComponent, ref xform))
            return;
        if (!_solutionContainer.TryGetSolution(reclaimer, reclaimerComponent.SolutionContainerId, out var outputSolution))
            return;

        efficiency *= reclaimerComponent.Efficiency;

        var totalChemicals = new Solution();

        if (Resolve(item, ref composition, false))
        {
            foreach (var (key, value) in composition.ChemicalComposition)
            {
                // TODO use ReagentQuantity
                totalChemicals.AddReagent(key, value * efficiency, false);
            }
        }

        // if the item we inserted has reagents, add it in.
        if (TryComp<SolutionContainerManagerComponent>(item, out var solutionContainer))
        {
            foreach (var (_, soln) in _solutionContainer.EnumerateSolutions((item, solutionContainer)))
            {
                var solution = soln.Comp.Solution;
                foreach (var quantity in solution.Contents)
                {
                    totalChemicals.AddReagent(quantity.Reagent.Prototype, quantity.Quantity * efficiency, false);
                }
            }
        }

        _solutionContainer.TryTransferSolution(outputSolution.Value, totalChemicals, totalChemicals.Volume);
        if (totalChemicals.Volume > 0)
        {
            _puddle.TrySpillAt(reclaimer, totalChemicals, out _, sound, transformComponent: xform);
        }
    }
}
