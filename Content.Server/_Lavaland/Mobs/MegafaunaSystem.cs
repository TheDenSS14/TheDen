// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Lavaland.Aggression;
using Content.Shared._Lavaland.Audio;
using Content.Shared.Mobs;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Player;

namespace Content.Server._Lavaland.Mobs;

public sealed class MegafaunaSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MegafaunaComponent, AttackedEvent>(OnAttacked);
        SubscribeLocalEvent<MegafaunaComponent, MobStateChangedEvent>(OnDeath);
    }

    public void OnAttacked<T>(EntityUid uid, T comp, ref AttackedEvent args) where T : MegafaunaComponent
    {
        if (!HasComp<MegafaunaWeaponLooterComponent>(args.Used))
            comp.CrusherOnly = false; // it's over...
    }

    public void OnDeath<T>(EntityUid uid, T comp, ref MobStateChangedEvent args) where T : MegafaunaComponent
    {
        var coords = Transform(uid).Coordinates;

        comp.CancelToken.Cancel();

        RaiseLocalEvent(uid, new MegafaunaKilledEvent());

        if (TryComp<BossMusicComponent>(uid, out var boss) &&
            TryComp<AggressiveComponent>(uid, out var aggresive))
        {
            var msg = new BossMusicStopEvent();
            foreach (var aggressor in aggresive.Aggressors)
            {
                if (!TryComp<ActorComponent>(aggressor, out var actor))
                    return;

                RaiseNetworkEvent(msg, actor.PlayerSession.Channel);
            }
        }

        if (comp.CrusherOnly && comp.CrusherLoot != null)
        {
            Spawn(comp.CrusherLoot, coords);
        }
        else if (comp.Loot != null)
        {
            Spawn(comp.Loot, coords);
        }

        QueueDel(uid);
    }
}
