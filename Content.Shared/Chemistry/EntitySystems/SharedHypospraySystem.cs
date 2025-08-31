// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Leon Friedrich
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Ygg01
// SPDX-FileCopyrightText: 2021 mirrorcult
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Plykiya
// SPDX-FileCopyrightText: 2024 beck-thompson
// SPDX-FileCopyrightText: 2025 CyberLanos
// SPDX-FileCopyrightText: 2025 Eagle-0
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Content.Shared.Forensics;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Events;
using System.Linq;
using Content.Shared._DV.Abilities.Chitinid;
using Content.Shared.Administration.Logs;
using Content.Shared.CombatMode;
using Content.Shared.DoAfter;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;


namespace Content.Shared.Chemistry.EntitySystems;

public sealed class SharedHypospraySystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = null!;
    [Dependency] private readonly SharedAudioSystem _audio = null!;
    [Dependency] private readonly ReactiveSystem _reactiveSystem = null!;
    [Dependency] private readonly SharedPopupSystem _popup = null!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainers = null!;
    [Dependency] private readonly MobStateSystem _mobState = null!;
    [Dependency] private readonly SharedCombatModeSystem _combatMode = null!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = null!;
    [Dependency] private readonly UseDelaySystem _useDelay = null!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HyposprayComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<HyposprayComponent, MeleeHitEvent>(OnAttack);
        SubscribeLocalEvent<HyposprayComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<HyposprayComponent, HyposprayDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<HyposprayComponent, GetVerbsEvent<AlternativeVerb>>(AddToggleModeVerb);
    }

    #region Ref Events
    private void OnUseInHand(Entity<HyposprayComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = TryDoInject(entity, args.User, args.User);
    }

    private void OnAfterInteract(Entity<HyposprayComponent> entity, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || args.Target == null)
            return;

        args.Handled = TryUseHypospray(entity, args.Target.Value, args.User);
    }

    private void OnAttack(Entity<HyposprayComponent> entity, ref MeleeHitEvent args)
    {
        if (args.Handled) // Goobstation
            return;

        if (args.HitEntities is [])
            return;

        args.Handled = TryDoInject(entity, args.HitEntities.First(), args.User);
    }

    private void OnDoAfter(Entity<HyposprayComponent> entity, ref HyposprayDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        var target = (EntityUid) args.Target!;
        var user = args.User;

        var (uid, component) = entity;

        if (!EligibleEntity(target, component))
            return;

        if (TryComp<UseDelayComponent>(uid, out var delayComp))
        {
            if (_useDelay.IsDelayed((uid, delayComp)))
                return;
        }

        string? msgFormat = null;

        // Self event
        var selfEvent = new SelfBeforeHyposprayInjectsEvent(user, entity.Owner, target);
        RaiseLocalEvent(user, selfEvent);

        if (selfEvent.Cancelled)
        {
            _popup.PopupClient(Loc.GetString(selfEvent.InjectMessageOverride ?? "hypospray-cant-inject", ("owner", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        target = selfEvent.TargetGettingInjected;

        if (!EligibleEntity(target, component))
            return;

        // Target event
        var targetEvent = new TargetBeforeHyposprayInjectsEvent(user, entity.Owner, target);
        RaiseLocalEvent(target, targetEvent);

        if (targetEvent.Cancelled)
        {
            _popup.PopupClient(Loc.GetString(targetEvent.InjectMessageOverride ?? "hypospray-cant-inject", ("owner", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        target = targetEvent.TargetGettingInjected;

        if (!EligibleEntity(target, component))
            return;

        // The target event gets priority for the overriden message.
        if (targetEvent.InjectMessageOverride != null)
            msgFormat = targetEvent.InjectMessageOverride;
        else if (selfEvent.InjectMessageOverride != null)
            msgFormat = selfEvent.InjectMessageOverride;
        else if (target == user)
            msgFormat = "hypospray-component-inject-self-message";

        if (!_solutionContainers.TryGetSolution(uid, component.SolutionName, out var hypoSpraySoln, out var hypoSpraySolution) || hypoSpraySolution.Volume == 0)
        {
            _popup.PopupClient(Loc.GetString("hypospray-component-empty-message"), target, user);
            return;
        }

        if (!_solutionContainers.TryGetInjectableSolution(target, out var targetSoln, out var targetSolution))
        {
            _popup.PopupClient(Loc.GetString("hypospray-cant-inject", ("target", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        _popup.PopupClient(Loc.GetString(msgFormat ?? "hypospray-component-inject-other-message", ("other", target)), target, user);

        if (target != user)
        {
            _popup.PopupEntity(Loc.GetString("hypospray-component-feel-prick-message"), target, target);
            // TODO: This should just be using melee attacks...
            // meleeSys.SendLunge(angle, user);
        }

        _audio.PlayPredicted(component.InjectSound, target, user);

        // Medipens and such use this system and don't have a delay, requiring extra checks
        // BeginDelay function returns if item is already on delay
        if (delayComp != null)
            _useDelay.TryResetDelay((uid, delayComp));

        var realTransferAmount = FixedPoint2.Min(component.TransferAmount, targetSolution.AvailableVolume);
        // Get transfer amount. May be smaller than component.TransferAmount if not enough room
        if (component.InjectMaxCapacity
        && _solutionContainers.TryGetSolution(entity.Owner, component.SolutionName, out _, out var solution))
            realTransferAmount = FixedPoint2.Min(solution.MaxVolume, targetSolution.AvailableVolume);

        if (realTransferAmount <= 0)
        {
            _popup.PopupClient(Loc.GetString("hypospray-component-transfer-already-full-message", ("owner", target)), target, user);
            return;
        }

        // Move units from attackSolution to targetSolution
        var removedSolution = _solutionContainers.SplitSolution(hypoSpraySoln.Value, realTransferAmount);

        if (!targetSolution.CanAddSolution(removedSolution))
            return;
        _reactiveSystem.DoEntityReaction(target, removedSolution, ReactionMethod.Injection);
        _solutionContainers.TryAddSolution(targetSoln.Value, removedSolution);

        var ev = new TransferDnaEvent { Donor = target, Recipient = uid};
        RaiseLocalEvent(target, ref ev);
        args.Handled = true;

        // same LogType as syringes...
        _adminLogger.Add(LogType.ForceFeed, $"{EntityManager.ToPrettyString(user):user} injected {EntityManager.ToPrettyString(target):target} with a solution {SharedSolutionContainerSystem.ToPrettyString(removedSolution):removedSolution} using a {EntityManager.ToPrettyString(uid):using}");
    }
    #endregion

    #region Draw/Inject
    private bool TryUseHypospray(Entity<HyposprayComponent> entity, EntityUid target, EntityUid user)
    {
        // if target is ineligible but is a container, try to draw from the container
        if (!EligibleEntity(target, entity)
            && _solutionContainers.TryGetDrawableSolution(target, out var drawableSolution, out _))
        {
            return TryDraw(entity, target, drawableSolution.Value, user);
        }

        return TryDoInject(entity, target, user);
    }

    private bool TryDoInject(Entity<HyposprayComponent> entity, EntityUid target, EntityUid user)
    {
        var (_, component) = entity;

        var doAfterDelay = TimeSpan.FromSeconds(0);

        if (!entity.Comp.BypassBlockInjection &&
            TryComp<BlockInjectionComponent>(target, out var blockComponent)) // DeltaV
        {
            var msg = Loc.GetString(
                $"injector-component-deny-{blockComponent.BlockReason}",
                ("target", Identity.Entity(target, EntityManager)));
            _popup.PopupClient(msg, target, user);
            return false;
        }

        // Is the target a mob and hypo isn't instant? If yes, use a do-after to give them time to respond.
        if (HasComp<MobStateComponent>(target)
            && component.InjectTime != 0f)
            doAfterDelay = GetInjectionTime(entity, user, target);

        var doAfterEventArgs = new DoAfterArgs(
            EntityManager,
            user,
            doAfterDelay,
            new HyposprayDoAfterEvent(),
            entity.Owner,
            target,
            user)
        {
            BreakOnMove = true,
            BreakOnWeightlessMove = false,
            BreakOnDamage = true,
            NeedHand = true,
            BreakOnHandChange = true,
            MovementThreshold = 0.1f,
        };
        return _doAfter.TryStartDoAfter(doAfterEventArgs);
    }

    private bool TryDraw(Entity<HyposprayComponent> entity, EntityUid target, Entity<SolutionComponent> targetSolution, EntityUid user)
    {
        if (!_solutionContainers.TryGetSolution(entity.Owner, entity.Comp.SolutionName, out var soln,
                out var solution) || solution.AvailableVolume == 0)
        {
            return false;
        }

        // Get transfer amount. May be smaller than _transferAmount if not enough room, also make sure there's room in the injector
        var realTransferAmount = FixedPoint2.Min(entity.Comp.TransferAmount, targetSolution.Comp.Solution.Volume,
            solution.AvailableVolume);

        if (realTransferAmount <= 0)
        {
            _popup.PopupClient(
                Loc.GetString("injector-component-target-is-empty-message",
                    ("target", Identity.Entity(target, EntityManager))),
                entity.Owner, user);
            return false;
        }

        var removedSolution = _solutionContainers.Draw(target, targetSolution, realTransferAmount);

        if (!_solutionContainers.TryAddSolution(soln.Value, removedSolution))
        {
            return false;
        }

        _popup.PopupClient(Loc.GetString("injector-component-draw-success-message",
            ("amount", removedSolution.Volume),
            ("target", Identity.Entity(target, EntityManager))), entity.Owner, user);
        return true;
    }

    private TimeSpan GetInjectionTime(Entity<HyposprayComponent> entity, EntityUid user, EntityUid target)
    {
        HyposprayComponent comp = entity;
        if (!_solutionContainers.TryGetSolution(entity.Owner, entity.Comp.SolutionName, out _, out var solution))
            return TimeSpan.FromSeconds(3);

        if (solution.Volume == 0)
        {
            return TimeSpan.FromSeconds(0);
        }

        var actualDelay = MathHelper.Max(TimeSpan.FromSeconds(comp.InjectTime), TimeSpan.FromSeconds(0));
        // additional delay is based on actual volume left to inject in hypospray when smaller than transfer amount
        var amountToInject = Math.Max(comp.TransferAmount.Float(), solution.Volume.Float());

        // Injections take 0.25 seconds longer per 5u of possible space/content
        actualDelay += TimeSpan.FromSeconds(amountToInject / 20);

        // Create a pop-up for the user
        _popup.PopupClient(Loc.GetString("injector-component-injecting-user"), target, user);

        var isTarget = user != target;

        if (isTarget)
        {
            // Create a pop-up for the target
            var userName = Identity.Entity(user, EntityManager);
            _popup.PopupEntity(Loc.GetString("injector-component-injecting-target",
                ("user", userName)), user, target);

            // Check if the target is incapacitated or in combat mode and modify time accordingly.
            if (_mobState.IsIncapacitated(target))
            {
                actualDelay /= 2.5f;
            }
            else if (_combatMode.IsInCombatMode(target))
            {
                // Slightly increase the delay when the target is in combat mode. Helps prevents cheese injections in
                // combat with fast syringes & lag.
                actualDelay += TimeSpan.FromSeconds(1);
            }

            // Add an admin log, using the "force feed" log type. It's not quite feeding, but the effect is the same.
            _adminLogger.Add(LogType.ForceFeed,
                $"{EntityManager.ToPrettyString(user):user} is attempting to inject {EntityManager.ToPrettyString(target):target} with a solution {SharedSolutionContainerSystem.ToPrettyString(solution):solution}");

        }
        else
        {
            // Self-injections take half as long.
            actualDelay /= 2;
            _adminLogger.Add(LogType.Ingestion,
                $"{EntityManager.ToPrettyString(user):user} is attempting to inject themselves with a solution {SharedSolutionContainerSystem.ToPrettyString(solution):solution}.");

        }
        return actualDelay;
    }

    private bool EligibleEntity(EntityUid entity, HyposprayComponent component) =>
        // TODO: Does checking for BodyComponent make sense as a "can be hypospray'd" tag?
        // In SS13 the hypospray ONLY works on mobs, NOT beakers or anything else.
        // But this is 14, we dont do what SS13 does just because SS13 does it.
        component.OnlyAffectsMobs
            ? HasComp<SolutionContainerManagerComponent>(entity) &&
            HasComp<MobStateComponent>(entity)
            : HasComp<SolutionContainerManagerComponent>(entity);

    #endregion

    #region verbs
    // <summary>
    // Uses the OnlyMobs field as a check to implement the ability
    // to draw from jugs and containers with the hypospray
    // Toggleable to allow people to inject containers if they prefer it over drawing
    // </summary>
    private void AddToggleModeVerb(Entity<HyposprayComponent> entity, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null || entity.Comp.InjectOnly)
            return;

        var (_, component) = entity;
        var user = args.User;
        var verb = new AlternativeVerb
        {
            Text = Loc.GetString("hypospray-verb-mode-label"),
            Act = () =>
            {
                ToggleMode(entity, user);
            }
        };
        args.Verbs.Add(verb);
    }

    private void ToggleMode(Entity<HyposprayComponent> entity, EntityUid user)
    {
        SetMode(entity, !entity.Comp.OnlyAffectsMobs);
        string msg = entity.Comp.OnlyAffectsMobs ? "hypospray-verb-mode-inject-mobs-only" : "hypospray-verb-mode-inject-all";
        _popup.PopupClient(Loc.GetString(msg), entity, user);
    }

    public void SetMode(Entity<HyposprayComponent> entity, bool onlyAffectsMobs)
    {
        if (entity.Comp.OnlyAffectsMobs == onlyAffectsMobs)
            return;

        entity.Comp.OnlyAffectsMobs = onlyAffectsMobs;
        Dirty(entity);
    }
    #endregion
}
