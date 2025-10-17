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

using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Chemistry;
using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Content.Shared.Forensics;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Events;
using Content.Server.Interaction;
using Content.Server.Body.Components;
using Robust.Shared.GameStates;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Abilities.Chitinid;
using Content.Shared.Chemistry.Reagent;
using Robust.Server.Audio;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Chat.Managers;
using Content.Shared.DoAfter;
using Content.Server.Chemistry.Components;
using Content.Shared.Chat;
using Content.Shared.Popups;
using Robust.Server.Player;


namespace Content.Server.Chemistry.EntitySystems;

public sealed class HypospraySystem : SharedHypospraySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly AtmosphereSystem _atmosphere = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly InteractionSystem _interaction = default!;
    [Dependency] protected readonly SharedPopupSystem Popup = default!;
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private const ChatChannel BlockInjectionDenyChannel = ChatChannel.Emotes;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HyposprayComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<HyposprayComponent, MeleeHitEvent>(OnAttack);
        SubscribeLocalEvent<HyposprayComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<HyposprayComponent, HyposprayDoAfterEvent>(OnDoAfter);
    }

    private void UseHypospray(Entity<HyposprayComponent> entity, EntityUid target, EntityUid user)
    {
        // if target is ineligible but is a container, try to draw from the container
        if (!EligibleEntity(target, EntityManager, entity)
            && _solutionContainers.TryGetDrawableSolution(target, out var drawableSolution, out _))
        {
            TryDraw(entity, target, drawableSolution.Value, user);
        }

        TryDoInject(entity, target, user);
    }

    private void OnUseInHand(Entity<HyposprayComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        TryDoInject(entity, args.User, args.User);
        args.Handled = true;
    }

    public void OnAfterInteract(Entity<HyposprayComponent> entity, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || args.Target == null)
            return;

        UseHypospray(entity, args.Target.Value, args.User);
        args.Handled = true;
    }

    public void OnAttack(Entity<HyposprayComponent> entity, ref MeleeHitEvent args)
    {
        if (args.Handled) // Goobstation
            return;

        if (!args.HitEntities.Any())
            return;

        TryDoInject(entity, args.HitEntities.First(), args.User);
    }

    public bool TryDoInject(Entity<HyposprayComponent> entity, EntityUid target, EntityUid user)
    {
        var (_, component) = entity;

        var doAfterDelay = TimeSpan.FromSeconds(0);

        if (!entity.Comp.BypassBlockInjection && TryComp<BlockInjectionComponent>(target, out var blockComponent)) // DeltaV
        {
            var msg = Loc.GetString($"injector-component-deny-{blockComponent.BlockReason}",
                ("target", Identity.Entity(target, EntityManager)));
            Popup.PopupEntity(msg, target, user);

            if (!_playerManager.TryGetSessionByEntity(target, out var session))
                return false;

            _chat.ChatMessageToOne(
                BlockInjectionDenyChannel,
                msg,
                msg,
                EntityUid.Invalid,
                false,
                session.Channel);
            return false;
        }

        // Is the target a mob and hypo isn't instant? If yes, use a do-after to give them time to respond.
        if ((HasComp<MobStateComponent>(target) || HasComp<BloodstreamComponent>(target))
            && component.InjectTime != 0f)
        {
            doAfterDelay = GetInjectionTime(entity, user, target);
        }
        // What even is this for? It doesn't even stop injection.
//        if (component.MaxPressure != float.MaxValue)
//        {
//            var mixture = _atmosphere.GetTileMixture(target);
//            if (mixture != null && mixture.Pressure > component.MaxPressure)
//            {
//                doAfterDelay = component.InjectTime;
//            }
//        }
        var doAfterEventArgs = new DoAfterArgs(EntityManager, user, doAfterDelay, new HyposprayDoAfterEvent(), entity.Owner, target, user)
        {
            BreakOnMove = true,
            BreakOnWeightlessMove = false,
            BreakOnDamage = true,
            NeedHand = true,
            BreakOnHandChange = true,
            MovementThreshold = 0.1f,
        };

        _doAfter.TryStartDoAfter(doAfterEventArgs);
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

        var actualDelay = MathHelper.Max(TimeSpan.FromSeconds(comp.InjectTime), TimeSpan.FromSeconds(1));
        // additional delay is based on actual volume left to inject in syringe when smaller than transfer amount
        var amountToInject = Math.Max(comp.TransferAmount.Float(), solution.Volume.Float());

        // Injections take 0.25 seconds longer per 5u of possible space/content
        actualDelay += TimeSpan.FromSeconds(amountToInject / 20);

        // Create a pop-up for the user
        Popup.PopupEntity(Loc.GetString("injector-component-injecting-user"), target, user);

        var isTarget = user != target;

        if (isTarget)
        {
            // Create a pop-up for the target
            var userName = Identity.Entity(user, EntityManager);
            Popup.PopupEntity(Loc.GetString("injector-component-injecting-target",
                ("user", userName)), user, target);

            // Check if the target is incapacitated or in combat mode and modify time accordingly.
            if (MobState.IsIncapacitated(target))
            {
                actualDelay /= 2.5f;
            }
            else if (Combat.IsInCombatMode(target))
            {
                // Slightly increase the delay when the target is in combat mode. Helps prevents cheese injections in
                // combat with fast syringes & lag.
                actualDelay += TimeSpan.FromSeconds(1);
            }

            // Add an admin log, using the "force feed" log type. It's not quite feeding, but the effect is the same.
            AdminLogger.Add(LogType.ForceFeed,
                $"{EntityManager.ToPrettyString(user):user} is attempting to inject {EntityManager.ToPrettyString(target):target} with a solution {SharedSolutionContainerSystem.ToPrettyString(solution):solution}");

        }
        else
        {
            // Self-injections take half as long.
            actualDelay /= 2;
            AdminLogger.Add(LogType.Ingestion,
                $"{EntityManager.ToPrettyString(user):user} is attempting to inject themselves with a solution {SharedSolutionContainerSystem.ToPrettyString(solution):solution}.");

        }
        return actualDelay;
    }

    private void OnDoAfter(Entity<HyposprayComponent> entity, ref HyposprayDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        var target = (EntityUid) args.Target!;
        var user = args.User;

        var (uid, component) = entity;

        if (!EligibleEntity(target, EntityManager, component))
            return;

        if (TryComp(uid, out UseDelayComponent? delayComp))
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
            _popup.PopupEntity(Loc.GetString(selfEvent.InjectMessageOverride ?? "hypospray-cant-inject", ("owner", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        target = selfEvent.TargetGettingInjected;

        if (!EligibleEntity(target, EntityManager, component))
            return;

        // Target event
        var targetEvent = new TargetBeforeHyposprayInjectsEvent(user, entity.Owner, target);
        RaiseLocalEvent(target, targetEvent);

        if (targetEvent.Cancelled)
        {
            _popup.PopupEntity(Loc.GetString(targetEvent.InjectMessageOverride ?? "hypospray-cant-inject", ("owner", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        target = targetEvent.TargetGettingInjected;

        if (!EligibleEntity(target, EntityManager, component))
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
            _popup.PopupEntity(Loc.GetString("hypospray-component-empty-message"), target, user);
            return;
        }

        if (!_solutionContainers.TryGetInjectableSolution(target, out var targetSoln, out var targetSolution))
        {
            _popup.PopupEntity(Loc.GetString("hypospray-cant-inject", ("target", Identity.Entity(target, EntityManager))), target, user);
            return;
        }

        _popup.PopupEntity(Loc.GetString(msgFormat ?? "hypospray-component-inject-other-message", ("other", target)), target, user);

        if (target != user)
        {
            _popup.PopupEntity(Loc.GetString("hypospray-component-feel-prick-message"), target, target);
            // TODO: This should just be using melee attacks...
            // meleeSys.SendLunge(angle, user);
        }

        _audio.PlayPvs(component.InjectSound, user);

        // Medipens and such use this system and don't have a delay, requiring extra checks
        // BeginDelay function returns if item is already on delay
        if (delayComp != null)
            _useDelay.TryResetDelay((uid, delayComp));

        var realTransferAmount = FixedPoint2.Min(component.TransferAmount, targetSolution.AvailableVolume);
        // Get transfer amount. May be smaller than component.TransferAmount if not enough room
        if (component.InjectMaxCapacity
        && _solutionContainers.TryGetSolution(entity.Owner, component.SolutionName, out var soln, out var solution))
        {
            realTransferAmount = FixedPoint2.Min(solution.MaxVolume, targetSolution.AvailableVolume);
        }

        if (realTransferAmount <= 0)
        {
            _popup.PopupEntity(Loc.GetString("hypospray-component-transfer-already-full-message", ("owner", target)), target, user);
            return;
        }

        // Move units from attackSolution to targetSolution
        var removedSolution = _solutionContainers.SplitSolution(hypoSpraySoln.Value, realTransferAmount);

        if (!targetSolution.CanAddSolution(removedSolution))
            return;
        _reactiveSystem.DoEntityReaction(target, removedSolution, ReactionMethod.Injection);
        _solutionContainers.TryAddSolution(targetSoln.Value, removedSolution);

        var ev = new TransferDnaEvent { Donor = target, Recipient = uid };
        RaiseLocalEvent(target, ref ev);
        args.Handled = true;

        // same LogType as syringes...
        _adminLogger.Add(LogType.ForceFeed, $"{EntityManager.ToPrettyString(user):user} injected {EntityManager.ToPrettyString(target):target} with a solution {SharedSolutionContainerSystem.ToPrettyString(removedSolution):removedSolution} using a {EntityManager.ToPrettyString(uid):using}");
    }

    private void TryDraw(Entity<HyposprayComponent> entity, Entity<BloodstreamComponent?> target, Entity<SolutionComponent> targetSolution, EntityUid user)
    {
        if (!_solutionContainers.TryGetSolution(entity.Owner, entity.Comp.SolutionName, out var soln,
                out var solution) || solution.AvailableVolume == 0)
        {
            return;
        }

        // Get transfer amount. May be smaller than _transferAmount if not enough room, also make sure there's room in the injector
        var realTransferAmount = FixedPoint2.Min(entity.Comp.TransferAmount, targetSolution.Comp.Solution.Volume,
            solution.AvailableVolume);

        if (realTransferAmount <= 0)
        {
            _popup.PopupEntity(
                Loc.GetString("injector-component-target-is-empty-message",
                    ("target", Identity.Entity(target, EntityManager))),
                entity.Owner, user);
            return;
        }

        var removedSolution = _solutionContainers.Draw(target.Owner, targetSolution, realTransferAmount);

        if (!_solutionContainers.TryAddSolution(soln.Value, removedSolution))
        {
            return;
        }

        _popup.PopupEntity(Loc.GetString("injector-component-draw-success-message",
            ("amount", removedSolution.Volume),
            ("target", Identity.Entity(target, EntityManager))), entity.Owner, user);
    }

    private bool EligibleEntity(EntityUid entity, IEntityManager entMan, HyposprayComponent component)
    {
        // TODO: Does checking for BodyComponent make sense as a "can be hypospray'd" tag?
        // In SS13 the hypospray ONLY works on mobs, NOT beakers or anything else.
        // But this is 14, we dont do what SS13 does just because SS13 does it.
        return component.OnlyAffectsMobs
            ? entMan.HasComponent<SolutionContainerManagerComponent>(entity) &&
              entMan.HasComponent<MobStateComponent>(entity)
            : entMan.HasComponent<SolutionContainerManagerComponent>(entity);
    }
}
