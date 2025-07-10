using Content.Server.Actions;
using Content.Server.Chat.Systems;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.NPC.Components;
using Content.Server.Popups;
using Content.Shared._DEN.Silicons.Bots.Components;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Silicons.Bots;
using Robust.Shared.Containers;

namespace Content.Server.Silicons.Bots;

public sealed class MedibotSystem : SharedMedibotSystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly HypospraySystem _hypospray = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MedibotInjectorComponent, HyposprayDoAfterEvent>(AfterInjected,
            after: [typeof(HypospraySystem)]);
        SubscribeLocalEvent<MedibotInjectorComponent, UseInHandEvent>(CancelUseInHand,
            before: [typeof(HypospraySystem)]);

    }

    private void AfterInjected(EntityUid uid, MedibotInjectorComponent injectorComponent,
        ref HyposprayDoAfterEvent args)
    {
        if (!args.Handled || args.Cancelled || injectorComponent.Medibot == null)
            return;

        _chat.TrySendInGameICMessage(injectorComponent.Medibot.Value,
            Loc.GetString("medibot-finish-inject"),
            InGameICChatType.Speak,
            hideChat: true,
            hideLog: true);
    }

    private void CancelUseInHand(Entity<MedibotInjectorComponent> injector, ref UseInHandEvent args)
    {
        args.Handled = true;
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
        var medibot = entity.Comp;

        if (HasComp<NPCRecentlyInjectedComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("medibot-error-injected-too-recently"), target, uid);
            return false;
        }

        if (!TryComp<MobStateComponent>(target, out var state)
            || !TryComp<DamageableComponent>(target, out var damage)
            || !TryGetTreatment(medibot, state.CurrentState, out var treatment)
            || !HasComp<EmaggedComponent>(uid) && !treatment.IsValid(damage.TotalDamage))
        {
            _popup.PopupEntity(Loc.GetString("medibot-error-invalid-treatment"), target, uid);
            return false;
        }

        if (sayTheLine)
            _chat.TrySendInGameICMessage(uid,
                Loc.GetString("medibot-start-inject"),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);

        _popup.PopupEntity(Loc.GetString("injector-component-injecting-user"), target, uid);
        // TODO: use hypospray function instead
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
