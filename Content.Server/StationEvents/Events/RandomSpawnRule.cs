// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._Lavaland.Procedural.Components;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;

namespace Content.Server.StationEvents.Events;

public sealed class RandomSpawnRule : StationEventSystem<RandomSpawnRuleComponent>
{
    private bool Filter(EntityUid map) => !HasComp<LavalandMapComponent>(map);

    protected override void Started(EntityUid uid, RandomSpawnRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, comp, gameRule, args);

        if (comp.StationExclusive)
        {
            var found = TryGetRandomStationData(out var station, filter: Filter);

            if (
                found &&
                station != null &&
                TryFindRandomTileOnStation(station.Value, out _, out _, out var coords))
            {
                Sawmill.Info($"Spawning {comp.Prototype} at {coords}");
                Spawn(comp.Prototype, coords);
            }
        }
        else
        {
            if (TryFindRandomTile(out _, out _, out _, out var coords))
            {
                Sawmill.Info($"Spawning {comp.Prototype} at {coords}");
                Spawn(comp.Prototype, coords);
            }
        }
    }
}
