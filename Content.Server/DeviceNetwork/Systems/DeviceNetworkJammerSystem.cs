// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.DeviceNetwork.Components;
using Content.Shared.DeviceNetwork.Components;
using Robust.Server.GameObjects;

namespace Content.Server.DeviceNetwork.Systems;

public sealed class DeviceNetworkJammerSystem : EntitySystem
{
    [Dependency] private TransformSystem _transform = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TransformComponent, BeforePacketSentEvent>(BeforePacketSent);
    }

    private void BeforePacketSent(EntityUid uid, TransformComponent xform, BeforePacketSentEvent ev)
    {
        if (ev.Cancelled)
            return;

        var query = EntityQueryEnumerator<DeviceNetworkJammerComponent, TransformComponent>();

        while (query.MoveNext(out _, out var jammerComp, out var jammerXform))
        {
            if (!jammerComp.JammableNetworks.Contains(ev.NetworkId))
                continue;

            if (jammerXform.Coordinates.InRange(EntityManager, _transform, ev.SenderTransform.Coordinates, jammerComp.Range)
                || jammerXform.Coordinates.InRange(EntityManager, _transform, xform.Coordinates, jammerComp.Range))
            {
                ev.Cancel();
                return;
            }
        }
    }

}
