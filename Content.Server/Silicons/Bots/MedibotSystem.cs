// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chat.Systems;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Hands.Systems;
using Content.Server.Interaction;
using Content.Server.NPC.Components;
using Content.Server.Popups;
using Content.Shared._DEN.Silicons.Bots.Components;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Damage;
using Content.Shared.Emag.Components;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Silicons.Bots;

namespace Content.Server.Silicons.Bots;

public sealed class MedibotSystem : SharedMedibotSystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly InteractionSystem _interaction = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MedibotInjectorComponent, HyposprayDoAfterEvent>(AfterInjected,
            after: [typeof(HypospraySystem)]);
        SubscribeLocalEvent<MedibotInjectorComponent, UseInHandEvent>(CancelUseInHand,
            before: [typeof(HypospraySystem)]);
        SubscribeLocalEvent<MedibotInjectorComponent, AfterInteractEvent>(OnAfterInteract,
            before: [typeof(HypospraySystem)]);
    }

    private void AfterInjected(Entity<MedibotInjectorComponent> injector, ref HyposprayDoAfterEvent args)
    {
        if (!args.Handled || args.Cancelled || !HasComp<MedibotComponent>(args.User))
            return;

        _chat.TrySendInGameICMessage(args.User,
            Loc.GetString("medibot-finish-inject"),
            InGameICChatType.Speak,
            hideChat: true,
            hideLog: true);
    }

    private void CancelUseInHand(Entity<MedibotInjectorComponent> injector, ref UseInHandEvent args)
    {
        args.Handled = true;
    }

    private void OnAfterInteract(Entity<MedibotInjectorComponent> injector, ref AfterInteractEvent args)
    {
        var medibot = args.User;

        if (!TryComp<MedibotComponent>(medibot, out var medibotComp)
            || !TryComp<HyposprayComponent>(args.Used, out var hypospray)
            || !_solutionContainer.TryGetSolution(args.Used, hypospray.SolutionName, out var injectorSolution)
            || !TryComp<MobStateComponent>(args.Target, out var state)
            || !TryGetTreatment(medibotComp, state.CurrentState, out var treatment))
            return;

        _solutionContainer.RemoveAllSolution(injectorSolution.Value);

        if (!CanInjectTarget((medibot, medibotComp), args.Target.Value, out var reason))
        {
            _popup.PopupEntity(reason, args.Target.Value, medibot);
            args.Handled = true;
            return;
        }

        _solutionContainer.TryAddReagent(injectorSolution.Value, treatment.Reagent, treatment.Quantity, out _);
    }

    /// <summary>
    ///     Checks if a target can be injected by this medibot.
    /// </summary>
    /// <param name="entity">The medibot performing the injection.</param>
    /// <param name="target">The target receiving treatment.</param>
    /// <param name="reason">The reason why the medibot cannot inject the target</param>
    /// <returns>Whether or not the medibot can inject a target.</returns>
    public bool CanInjectTarget(Entity<MedibotComponent> entity, EntityUid target, out string reason)
    {
        var uid = entity.Owner;
        var medibot = entity.Comp;
        reason = "";

        if (!_interaction.TryGetUsedEntity(uid, out var used, true)
            || !HasComp<MedibotInjectorComponent>(used))
        {
            reason = Loc.GetString("medibot-error-no-injection-tool");
            return false;
        }

        if (!_solutionContainer.TryGetInjectableSolution(target, out var _, out var _))
        {
            reason = Loc.GetString("medibot-error-no-bloodstream");
            return false;
        }

        if (HasComp<NPCRecentlyInjectedComponent>(target))
        {
            reason = Loc.GetString("medibot-error-injected-too-recently");
            return false;
        }

        if (!TryComp<MobStateComponent>(target, out var state)
            || !TryComp<DamageableComponent>(target, out var damage)
            || !TryGetTreatment(medibot, state.CurrentState, out var treatment)
            || !HasComp<EmaggedComponent>(uid) && !treatment.IsValid(damage.TotalDamage))
        {
            reason = Loc.GetString("medibot-error-invalid-treatment");
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Ensures that the medibot can inject the patient, and then attempts to do so with a DoAfter.
    /// </summary>
    /// <param name="entity">The medibot entity performing the injection.</param>
    /// <param name="target">The ID of the target.</param>
    /// <param name="sayTheLine">Whether the medibot should say "Hold still, please."</param>
    /// <returns>Whether or not the DoAfter was started successfully.</returns>
    public bool TryInjectTarget(Entity<MedibotComponent> entity, EntityUid target, bool sayTheLine = false)
    {
        var uid = entity.Owner;
        var targetCoords = Transform(target).Coordinates;

        if (!CanInjectTarget(entity, target, out var reason))
        {
            _popup.PopupEntity(reason, target, uid);
            return false;
        }

        if (!_interaction.TryGetUsedEntity(uid, out var used, true)
            || !_interaction.InteractUsing(
            uid,
            used.Value,
            target,
            targetCoords,
            checkCanInteract: true,
            checkCanUse: true))
            return false;

        if (sayTheLine)
            _chat.TrySendInGameICMessage(uid,
                Loc.GetString("medibot-start-inject"),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);

        _popup.PopupEntity(Loc.GetString("injector-component-injecting-user"), target, uid);
        return true;
    }

    /// <summary>
    ///     Ensures that the medibot can inject the patient, and then attempts to do so with a DoAfter.
    /// </summary>
    /// <param name="uid">The ID of the entity performing the injection.</param>
    /// <param name="target">The ID of the target.</param>
    /// <param name="sayTheLine">Whether the medibot should say "Hold still, please."</param>
    /// <param name="medibot">Optional, the MedibotComponent to use.</param>
    /// <returns>Whether or not the DoAfter was started successfully.</returns>
    public bool TryInjectTarget(EntityUid uid,
        EntityUid target,
        bool sayTheLine = false,
        MedibotComponent? medibot = null)
    {
        if (!Resolve(uid, ref medibot))
            return false;

        var ent = new Entity<MedibotComponent>(uid, medibot);
        return TryInjectTarget(ent, target, sayTheLine);
    }
}
