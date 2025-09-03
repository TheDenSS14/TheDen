// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Nim
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 empty0set
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.StationEvents.Components;
using Content.Server.Antag;
using Content.Server.Pinpointer;
using Content.Shared.EntityTable;
using Content.Shared.GameTicking.Components;
using Content.Server.Station.Components;
using Content.Server.Announcements.Systems;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Audio;
using Content.Server.Chat.Systems;

namespace Content.Server.StationEvents.Events;

/// <summary>
/// DeltaV: Reworked vent critters to spawn a number of mobs at a single telegraphed location.
/// This gives players time to run away and let sec do their job.
/// </summary>
/// <remarks>
/// This entire file is rewritten, ignore upstream changes.
/// </remarks>
public sealed class VentCrittersRule : StationEventSystem<VentCrittersRuleComponent>
{
    /*
     * DO NOT COPY PASTE THIS TO MAKE YOUR MOB EVENT.
     * USE THE PROTOTYPE.
     */

    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly EntityTableSystem _entityTable = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly NavMapSystem _navMap = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly AnnouncerSystem _announcer = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;

    private List<Tuple<EntityUid, VentCritterSpawnLocationComponent, EntityCoordinates>> _locations = new();
    private Tuple<EntityUid, VentCritterSpawnLocationComponent, EntityCoordinates>? _location;

    protected override void Added(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        PickLocation(comp);
        if (_location == null)
        {
            ForceEndSelf(uid, gameRule);
            return;
        }

        if (comp.Location is not { } coords)
        {
            ForceEndSelf(uid, gameRule);
            return;
        }

        var mapCoords = _transform.ToMapCoordinates(coords);
        if (!_navMap.TryGetNearestBeacon(mapCoords, out var beacon, out _))
            return;

        Audio.PlayPvs(comp.Sound, _location.Item1, AudioParams.Default.AddVolume(250));
        base.Added(uid, comp, gameRule, args);

        _chatSystem.TrySendInGameICMessage(_location.Item1, "emits an ominous rumbling sound...", Shared.Chat.InGameICChatType.Emote, Shared.Chat.ChatTransmitRange.Normal, false, null, null, "nearby vent", false, true);

    }

    protected override void Ended(EntityUid uid, VentCrittersRuleComponent comp, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        if (_location == null)
            return;

        base.Ended(uid, comp, gameRule, args);

        if (comp.Location is not { } coords)
            return;

        _chatSystem.TrySendInGameICMessage(_location.Item1, "screeches as something bursts free in a cloud of dust!", Shared.Chat.InGameICChatType.Emote, Shared.Chat.ChatTransmitRange.Normal, false, null, null, "nearby vent", false, true);

        Spawn("AdminInstantEffectSmoke10", _location.Item3);

        SpawnCritters(comp, coords);
    }

    private void SpawnCritters(VentCrittersRuleComponent comp, EntityCoordinates coords)
    {
        var players = _antag.GetTotalPlayerCount(_player.Sessions);
        var min = comp.Min * players / comp.PlayerRatio;
        var max = comp.Max * players / comp.PlayerRatio;
        var count = Math.Max(RobustRandom.Next(min, max), 1);

        for (int i = 0; i < count; i++)
        {
            foreach (var spawn in _entityTable.GetSpawns(comp.Table))
            {
                Spawn(spawn, coords);
            }
        }

        if (comp.SpecialEntries.Count == 0)
            return;

        // guaranteed spawn
        var specialEntry = RobustRandom.Pick(comp.SpecialEntries);
        Spawn(specialEntry.PrototypeId, coords);
    }


    private void PickLocation(VentCrittersRuleComponent comp)
    {
        if (!TryGetRandomStation(out var station))
            return;

        var locations = EntityQueryEnumerator<VentCritterSpawnLocationComponent, TransformComponent>();
        _locations.Clear();
        while (locations.MoveNext(out var uid, out var spawnLocation, out var transform))
        {
            if (CompOrNull<StationMemberComponent>(transform.GridUid)?.Station == station && spawnLocation.CanSpawn)
            {
                _locations.Add(new Tuple<EntityUid, VentCritterSpawnLocationComponent, EntityCoordinates>(uid, spawnLocation, transform.Coordinates));
            }
        }

        if (_locations.Count == 0)
            return;

        _location = RobustRandom.Pick(_locations);

        comp.Location = _location.Item3;
    }
}
