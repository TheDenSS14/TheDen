// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Roboisseur.Roboisseur;
using Content.Shared.Item;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests._DV;

[TestFixture]
[TestOf(typeof(RoboisseurSystem))]
public sealed class RoboisseurTest
{
    [Test]
    public async Task AllRoboisseurItemsExist()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        // Per RobustIntegrationTest.cs, wait until state is settled to access it.
        await server.WaitIdleAsync();

        var mapManager = server.ResolveDependency<IMapManager>();
        var prototypeManager = server.ResolveDependency<IPrototypeManager>();
        var entityManager = server.ResolveDependency<IEntityManager>();
        var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
        var componentFactory = server.ResolveDependency<IComponentFactory>();

        var roboisseurSystem = entitySystemManager.GetEntitySystem<RoboisseurSystem>();

        var testMap = await pair.CreateTestMap();

        await server.WaitAssertion(() =>
        {
            var grid = mapManager.CreateGridEntity(testMap.MapId);
            var coord = new EntityCoordinates(grid.Owner, 0, 0);
            var protos = prototypeManager.EnumeratePrototypes<EntityPrototype>()
                .Where(p => !p.Abstract
                    && !pair.IsTestPrototype(p)
                    && p.TryGetComponent<RoboisseurComponent>(out _, componentFactory))
                .Select(p => p.ID)
                .ToList();

            Console.WriteLine($"Found {protos.Count} entity prototypes with RoboisseurComponent for testing.");

            foreach (var protoId in protos)
            {
                var ent = entityManager.SpawnEntity(protoId, coord);
                var roboisseurComponent = entityManager.GetComponent<RoboisseurComponent>(ent);
                CheckRoboisseurItemsExist(roboisseurComponent, protoId, coord, prototypeManager, entityManager);
            }

            // Because Server/Client pairs can be re-used between Tests, we
            // need to clean up anything that might affect other tests,
            // otherwise this pair cannot be considered clean, and the
            // CleanReturnAsync call would need to be removed.
            mapManager.DeleteMap(testMap.MapId);
        });

        await pair.CleanReturnAsync();
    }

    private void CheckRoboisseurItemsExist(RoboisseurComponent roboisseurComponent,
        EntProtoId protoId,
        EntityCoordinates coordinates,
        IPrototypeManager prototypeManager,
        IEntityManager entityManager)
    {
        var allProtos = roboisseurComponent.Tier2Protos
            .Concat(roboisseurComponent.Tier3Protos)
            .Concat(roboisseurComponent.RobossuierRewards);
        var enumerable = allProtos as string[] ?? allProtos.ToArray();
        var blacklistedProtos = roboisseurComponent.BlacklistedProtos;

        Assert.That(enumerable.Any(), $"RoboisseurComponent in {protoId} has no valid prototypes!");

        foreach (var proto in enumerable)
        {
            Assert.That(prototypeManager.TryIndex(proto, out var _),
                $"RoboisseurComponent in {protoId} has invalid prototype {proto}!");

            var spawned = entityManager.SpawnEntity(proto, coordinates);

            Assert.That(entityManager.HasComponent<ItemComponent>(spawned),
                $"RoboisseurComponent in {protoId} can request non-item {proto}!");
        }

        foreach (var proto in blacklistedProtos)
        {
            Assert.That(prototypeManager.TryIndex(proto, out var _),
                $"RoboisseurComponent in {protoId} has invalid prototype {proto} in blacklist!");
        }
    }
}
