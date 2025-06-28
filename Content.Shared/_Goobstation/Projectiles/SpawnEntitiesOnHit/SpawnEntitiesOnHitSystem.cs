// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Projectiles;

namespace Content.Shared._Goobstation.Projectiles.SpawnEntitiesOnHit;

public sealed class SpawnEntitiesOnHitSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpawnEntitiesOnHitComponent, ProjectileHitEvent>(OnHit);
    }

    private void OnHit(Entity<SpawnEntitiesOnHitComponent> ent, ref ProjectileHitEvent args)
    {
        var coords = Transform(ent).Coordinates;
        for (var i = 0; i < ent.Comp.Amount; i++)
        {
            Spawn(ent.Comp.Proto, coords);
        }
    }
}
