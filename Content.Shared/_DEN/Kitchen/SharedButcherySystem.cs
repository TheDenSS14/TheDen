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

    public void SpawnButcherableProducts(EntityUid uid, ButcherableComponent butcher, out EntityUid lastEntity)
    {
        var spawnEntities = EntitySpawnCollection.GetSpawns(butcher.SpawnedEntities, _robustRandom);
        var coords = _transform.GetMapCoordinates(uid);

        lastEntity = default!;
        foreach (var proto in spawnEntities)
        {
            // distribute the spawned items randomly in a small radius around the origin
            lastEntity = Spawn(proto, coords.Offset(_robustRandom.NextVector2(0.25f)));

            if (butcher.SpawnedInheritFreshness)
            {
                _rotting.TransferFreshness(uid, lastEntity, true, butcher);
                _rotting.TransferRotStage(uid, lastEntity, true);
            }
        }
    }
}
