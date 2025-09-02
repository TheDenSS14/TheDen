using System.Collections.Generic;
using System.Linq;
using Content.Client.Clickable;
using Content.Client.Markers;
using Content.Server.Explosion.Components;
using Content.Server.Humanoid.Components;
using Content.Server.Spawners.Components;
using Content.Shared.Prototypes;
using Content.Shared.Wall;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Spawners;

namespace Content.IntegrationTests.Tests._DEN.Entities;

/// <summary>
///     Tests if all entities (that are not hidden from the spawn menu) are things that reasonably should be allowed to
///     be spawned in from the menu.
/// </summary>
[TestFixture]
[TestOf(typeof(EntityPrototype))]
public sealed class EntitySpawnMenuTest
{
    private readonly HashSet<Type> _allowedServerComponents = new()
    {
        typeof(PhysicsComponent),
        typeof(RandomHumanoidSpawnerComponent),
        typeof(TriggerOnSpawnComponent),
        typeof(EntityTableSpawnerComponent),
        typeof(RandomSpawnerComponent),
        typeof(TimedDespawnComponent),
        typeof(SpawnOnDespawnComponent),
        typeof(WallMountComponent),
    };

    private readonly HashSet<Type> _allowedClientComponents = new()
    {
        typeof(MarkerComponent),
        typeof(ClickableComponent),
    };

    private readonly HashSet<string> _ignoredPrototypes = ["PointingArrow", "Audio"];

    [Test]
    public async Task AllNonWorldEntitiesHidden()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var client = pair.Client;
        var serverCompFac = server.ResolveDependency<IComponentFactory>();
        var clientCompFac = client.ResolveDependency<IComponentFactory>();
        var failingPrototypes = new List<string>();

        foreach (var p in server.ProtoMan.EnumeratePrototypes<EntityPrototype>()
                    .Where(p => !(p.Abstract
                        || pair.IsTestPrototype(p)
                        || p.HideSpawnMenu
                        || p.Categories.Any(c => c.ID == "Debug")
                        || _ignoredPrototypes.Contains(p.ID))))
        {
            // presumably anything that has a defined "placement mode" is supposed to be placeable
            if (p.PlacementMode != "PlaceFree"
                || _allowedServerComponents.Any(allowedType => p.HasComponent(allowedType, serverCompFac))
                || _allowedClientComponents.Any(allowedType => p.HasComponent(allowedType, clientCompFac)))
                continue;

            failingPrototypes.Add(p.ID);
        }

        Assert.That(failingPrototypes,
            Is.Empty,
            "The following prototypes lack common 'world entity' components - do they need [HideSpawnMenu] or [Debug]?"
            + "\n  " + string.Join("\n  ", failingPrototypes));

        await pair.CleanReturnAsync();
    }
}
