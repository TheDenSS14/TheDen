// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.GameTicking.Components;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

public sealed class MidRoundAntagRule : StationEventSystem<MidRoundAntagRuleComponent>
{
    protected override void Started(EntityUid uid, MidRoundAntagRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        if (!TryGetRandomStation(out var station))
            return;

        var ev = new BeforeMidRoundAntagSpawnEvent((uid, component));
        RaiseLocalEvent(ref ev);

        if (ev.Cancelled)
            return;

        var spawnLocations = FindSpawns(station.Value);
        if (spawnLocations.Count == 0)
        {
            Log.Warning("Couldn't find any midround antag spawners or vent critter spawners, not spawning an antag.");
            return;
        }

        var spawn = RobustRandom.Pick(spawnLocations);

        var proto = component.Spawner;
        Log.Info($"Spawning midround antag {proto} at {spawn.Coordinates}");
        Spawn(proto, spawn.Coordinates);
    }

    private List<TransformComponent> FindSpawns(EntityUid station)
    {
        var spawns = new List<TransformComponent>();
        var query = EntityQueryEnumerator<MidRoundAntagSpawnLocationComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var xform))
        {
            if (StationSystem.GetOwningStation(uid, xform) == station && xform.GridUid != null)
                spawns.Add(xform);
        }

        // if there are any midround antag spawns mapped, use them
        if (spawns.Count > 0)
            return spawns;

        // otherwise, fall back to vent critter spawns
        Log.Info($"Station {ToPrettyString(station):station} has no midround antag spawnpoints mapped, falling back. Please map them!");
        var fallbackQuery = EntityQueryEnumerator<VentCritterSpawnLocationComponent, TransformComponent>();
        while (fallbackQuery.MoveNext(out var uid, out _, out var xform))
        {
            if (StationSystem.GetOwningStation(uid, xform) == station && xform.GridUid != null)
                spawns.Add(xform);
        }

        return spawns;
    }
}

[ByRefEvent]
public record struct BeforeMidRoundAntagSpawnEvent(
    Entity<MidRoundAntagRuleComponent> MidRoundAntagRule,
    bool Cancelled = false);
