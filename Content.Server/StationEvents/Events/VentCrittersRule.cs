// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Nim
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 Eightballll
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 empty0set
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.StationEvents.Components;
using Content.Server.Antag;
using Content.Shared.EntityTable;
using Content.Shared.GameTicking.Components;
using Content.Shared.Station.Components;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Audio;
using Content.Server.Chat.Systems;
using Content.Shared.Pinpointer;
using System.Linq;

namespace Content.Server.StationEvents.Events;

/// <summary>
/// DeltaV: Reworked vent critters to spawn a number of mobs at a single telegraphed location.
/// This gives players time to run away and let sec do their job.
/// </summary>
/// <remarks>
/// This entire file is rewritten, ignore upstream changes.
/// </remarks>
///
public sealed class VentCrittersRule : StationEventSystem<VentCrittersRuleComponent>
{
    /*
     * DO NOT COPY PASTE THIS TO MAKE YOUR MOB EVENT.
     * USE THE PROTOTYPE.
     */

    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly EntityTableSystem _entityTable = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!; // DEN

    private List<VentCritterLocationData> _locations = new();
    private Entity<TransformComponent> _selectedBeacon = new();

    protected override void Added(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        PickSpawnLocations();

        if (_locations.Count == 0)
        {
            ForceEndSelf(uid, gameRule);
            return;
        }

        foreach (var location in _locations)
        {
            _chatSystem.TrySendInGameICMessage(location.LocationUid, "emits an ominous rumbling sound...", Shared.Chat.InGameICChatType.Emote, Shared.Chat.ChatTransmitRange.Normal, false, null, null, "nearby vent", false, true);
        }

        var audio = AudioParams.Default;
        audio.Volume = 200;
        audio.RolloffFactor = 0.01f;
        audio.ReferenceDistance = 400;

        Audio.PlayPvs(comp.Sound, _selectedBeacon.Comp.Coordinates, audio);

        base.Added(uid, comp, gameRule, args);
    }

    protected override void Ended(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        if (_locations.Count == 0)
            return;

        var players = _antag.GetTotalPlayerCount(_player.Sessions);
        var min = comp.Min * players / comp.PlayerRatio;
        var max = comp.Max * players / comp.PlayerRatio;
        var maxSpawns = Math.Max(RobustRandom.Next(min, max), 1);

        var spawnsPerVent = Math.Max((maxSpawns / _locations.Count), 1);

        foreach (var location in _locations)
        {
            _chatSystem.TrySendInGameICMessage(location.LocationUid, "screeches as something bursts free in a cloud of dust!", Shared.Chat.InGameICChatType.Emote, Shared.Chat.ChatTransmitRange.Normal, false, null, null, "nearby vent", false, true);

            Spawn("AdminInstantEffectSmoke10", location.Coordinates); // Dust effect

            SpawnCritters(comp, location, spawnsPerVent);
        }

        base.Ended(uid, comp, gameRule, args);
    }

    private void SpawnCritters(VentCrittersRuleComponent comp, VentCritterLocationData vent, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            foreach (var spawn in _entityTable.GetSpawns(comp.Table))
            {
                Spawn(spawn, vent.Coordinates);
            }
        }

        // guaranteed spawn
        if (comp.SpecialEntries.Count > 0)
        {
            var specialEntry = RobustRandom.Pick(comp.SpecialEntries);
            Spawn(specialEntry.PrototypeId, vent.Coordinates);
        }
    }

    private void PickSpawnLocations()
    {
        if (!TryGetRandomStation(out var station))
            return;

        _locations.Clear();

        // Get all beacons on station
        var beacons = EntityQueryEnumerator<NavMapBeaconComponent, TransformComponent>();
        var beaconList = new List<Entity<TransformComponent>>();

        while (beacons.MoveNext(out var beaconUid, out var navMapBeacon, out var beaconPosition))
        {
            // Check that the beacon is actually on the station, if so add to the list
            if (CompOrNull<StationMemberComponent>(beaconPosition.GridUid)?.Station == station)
            {
                beaconList.Add((beaconUid, beaconPosition));
            }
        }
        // Grab a random beacon from our list
        if (!beaconList.Any())
            return;

        _selectedBeacon = RobustRandom.Pick(beaconList);

        // 10 tile range is purely arbitrary, it would be better to pick vents up to a maximum value instead but
        var ventsInRange = _lookup.GetEntitiesInRange<VentCritterSpawnLocationComponent>(_selectedBeacon.Comp.Coordinates, 10).Where(x => x.Comp.CanSpawn);

        foreach (var vent in ventsInRange)
        {
            _locations.Add(new VentCritterLocationData(vent.Owner, vent.Comp, Transform(vent.Owner).Coordinates));
        }
    }
}

/// <summary>
/// Contains location data for the vents that have been selected to spawn critters
/// </summary>
public sealed class VentCritterLocationData
{
    public EntityUid LocationUid;
    public VentCritterSpawnLocationComponent SpawnLocationComponent;
    public EntityCoordinates Coordinates;

    public VentCritterLocationData(EntityUid uid, VentCritterSpawnLocationComponent spawnComp, EntityCoordinates coords)
    {
        LocationUid = uid;
        SpawnLocationComponent = spawnComp;
        Coordinates = coords;
    }
}
