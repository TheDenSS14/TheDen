// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Hands;

[TestFixture]
public sealed class HandTests
{
    [Test]
    public async Task TestPickupDrop()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings
        {
            Connected = true,
            DummyTicker = false
        });
        var server = pair.Server;

        var entMan = server.ResolveDependency<IEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();
        var mapMan = server.ResolveDependency<IMapManager>();
        var sys = entMan.System<SharedHandsSystem>();
        var tSys = entMan.System<TransformSystem>();

        var data = await pair.CreateTestMap();
        await pair.RunTicksSync(5);

        EntityUid item = default;
        EntityUid player = default;
        HandsComponent hands = default!;
        await server.WaitPost(() =>
        {
            player = playerMan.Sessions.First().AttachedEntity!.Value;
            var xform = entMan.GetComponent<TransformComponent>(player);
            item = entMan.SpawnEntity("Crowbar", tSys.GetMapCoordinates(player, xform: xform));
            hands = entMan.GetComponent<HandsComponent>(player);
            sys.TryPickup(player, item, hands.ActiveHand!);
        });

        // run ticks here is important, as errors may happen within the container system's frame update methods.
        await pair.RunTicksSync(5);
        Assert.That(hands.ActiveHandEntity, Is.EqualTo(item));

        await server.WaitPost(() =>
        {
            sys.TryDrop(player, item, null!);
        });

        await pair.RunTicksSync(5);
        Assert.That(hands.ActiveHandEntity, Is.Null);

        await server.WaitPost(() => mapMan.DeleteMap(data.MapId));
        await pair.CleanReturnAsync();
    }
}
