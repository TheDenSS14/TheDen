// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Spawners.Components;
using Robust.Shared.Spawners;

namespace Content.Server.Spawners.EntitySystems;

public sealed class SpawnOnDespawnSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpawnOnDespawnComponent, TimedDespawnEvent>(OnDespawn);
    }

    private void OnDespawn(EntityUid uid, SpawnOnDespawnComponent comp, ref TimedDespawnEvent args)
    {
        if (!TryComp(uid, out TransformComponent? xform))
            return;

        // Lavaland Change start
        if (comp.Prototype != null)
            Spawn(comp.Prototype, xform.Coordinates);
        // Lavaland Change end

        // Lavaland Change start
        // make it spawn more (without intrusion)
        foreach (var prot in comp.Prototypes)
            Spawn(prot, xform.Coordinates);
        // Lavaland Change end
    }
}
