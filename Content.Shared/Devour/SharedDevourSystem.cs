// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 PilgrimViis <PilgrimViis@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 RedFoxIV <38788538+RedFoxIV@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DEN.Devourable;
using Content.Shared.Actions;
using Content.Shared.Consent;
using Content.Shared.Damage;
using Content.Shared.Devour.Components;
using Content.Shared.DoAfter;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;

namespace Content.Shared.Devour;

public abstract class SharedDevourSystem : EntitySystem
{
    [Dependency] protected readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] protected readonly SharedContainerSystem ContainerSystem = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DevourerComponent, MapInitEvent>(OnInit);
        SubscribeLocalEvent<DevourerComponent, DevourActionEvent>(OnDevourAction);
    }

    protected void OnInit(EntityUid uid, DevourerComponent component, MapInitEvent args)
    {
        //Devourer doesn't actually chew, since he sends targets right into his stomach.
        //I did it mom, I added ERP content into upstream. Legally!
        component.Stomach = ContainerSystem.EnsureContainer<Container>(uid, "stomach");

        _actionsSystem.AddAction(uid, ref component.DevourActionEntity, component.DevourAction);
    }

    /// <summary>
    /// The devour action
    /// </summary>
    protected void OnDevourAction(EntityUid uid, DevourerComponent component, DevourActionEvent args)
    {
        if (args.Handled || _whitelistSystem.IsWhitelistFailOrNull(component.Whitelist, args.Target))
            return;

        args.Handled = true;
        var target = args.Target;

        // Structure and mob devours handled differently.
        if (TryComp(target, out MobStateComponent? targetState))
        {
            HandleMobState((uid, component), target, targetState);
            return;
        }

        _popupSystem.PopupClient(Loc.GetString("devour-action-popup-message-structure"), uid, uid);

        if (component.SoundStructureDevour != null)
            _audioSystem.PlayPredicted(component.SoundStructureDevour, uid, uid, component.SoundStructureDevour.Params);

        _doAfterSystem.TryStartDoAfter(
            new(
                EntityManager,
                uid,
                component.StructureDevourTime,
                new DevourDoAfterEvent(true),
                uid,
                target: target,
                used: uid)
        {
            BreakOnMove = true,
        });
    }

    private void HandleMobState(Entity<DevourerComponent> ent, EntityUid target, MobStateComponent? targetState)
    {
        TryComp<DevourableComponent>(target, out var devourable);

        if (!TryComp<DamageableComponent>(ent, out var damageable))
            return;

        switch (targetState?.CurrentState)
        {
            case MobState.Critical:
            case MobState.Dead:
                if (devourable != null && devourable.AttemptedDevouring)
                    return;

                var isDevourable = true;

                if (devourable != null && !devourable.IsDevourable)
                {
                    isDevourable = false;
                    devourable.AttemptedDevouring = true;

                    _damageableSystem.TryChangeDamage(ent.Owner, ent.Comp.HealDamage, true, false, damageable);
                    _popupSystem.PopupClient(Loc.GetString("devour-action-popup-message-fail-no-consent"), ent, ent);
                }

                _doAfterSystem.TryStartDoAfter(new(EntityManager, ent, ent.Comp.DevourTime, new DevourDoAfterEvent(isDevourable), ent, target: target, used: ent)
                {
                    BreakOnMove = true,
                });
                break;
            default:
                _popupSystem.PopupClient(Loc.GetString("devour-action-popup-message-fail-target-alive"), ent, ent);
                break;
        }
    }
}


public sealed partial class DevourActionEvent : EntityTargetActionEvent { }

[Serializable, NetSerializable]
public sealed partial class DevourDoAfterEvent : SimpleDoAfterEvent
{
    public bool AllowDevouring { get; set; }

    public DevourDoAfterEvent(bool allowDevouring) : this()
    {
        AllowDevouring = allowDevouring;
    }
}

[Serializable, NetSerializable]
public enum FoodPreference : byte
{
    Humanoid = 0,
    All = 1
}
