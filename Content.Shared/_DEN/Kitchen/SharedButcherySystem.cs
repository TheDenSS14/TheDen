// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos.Rotting;
using Content.Shared.Nutrition.Components;
using Content.Shared.Storage;
using Robust.Shared.Random;

namespace Content.Shared._DEN.Kitchen;

public sealed class SharedButcherySystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly SharedRottingSystem _rotting = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public EntityUid? SpawnButcherableProducts(EntityUid uid, ButcherableComponent butcher)
    {
        var spawnEntities = EntitySpawnCollection.GetSpawns(butcher.SpawnedEntities, _robustRandom);
        var coords = _transform.GetMapCoordinates(uid);
        EntityUid? lastEntity = null;

        foreach (var proto in spawnEntities)
        {
            // distribute the spawned items randomly in a small radius around the origin
            lastEntity = Spawn(proto, coords.Offset(_robustRandom.NextVector2(0.25f)));

            if (butcher.SpawnedInheritFreshness)
            {
                _rotting.TransferFreshness(uid, lastEntity.Value, true, butcher);
                _rotting.TransferRotStage(uid, lastEntity.Value, true);
            }
        }

        return lastEntity;
    }
}
