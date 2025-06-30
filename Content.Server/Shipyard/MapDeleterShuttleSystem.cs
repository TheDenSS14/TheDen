// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Shuttles.Events;

namespace Content.Server.Shipyard;

public sealed class MapDeleterShuttleSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MapDeleterShuttleComponent, FTLStartedEvent>(OnFTLStarted);
    }

    private void OnFTLStarted(Entity<MapDeleterShuttleComponent> ent, ref FTLStartedEvent args)
    {
        if (ent.Comp.Enabled)
            Del(args.FromMapUid);
        RemComp<MapDeleterShuttleComponent>(ent); // prevent the shuttle becoming a WMD
    }

    public void Enable(EntityUid shuttle)
    {
        EnsureComp<MapDeleterShuttleComponent>(shuttle).Enabled = true;
    }
}
