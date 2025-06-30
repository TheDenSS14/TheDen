// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 zelezniciar1 <39102800+zelezniciar1@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Pinpointer;
using Robust.Shared.GameStates;

namespace Content.Client.Pinpointer;

public sealed partial class NavMapSystem : SharedNavMapSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NavMapComponent, ComponentHandleState>(OnHandleState);
    }

    private void OnHandleState(EntityUid uid, NavMapComponent component, ref ComponentHandleState args)
    {
        Dictionary<Vector2i, int[]> modifiedChunks;
        Dictionary<NetEntity, NavMapBeacon> beacons;
        Dictionary<NetEntity, NavMapRegionProperties> regions;

        switch (args.Current)
        {
            case NavMapDeltaState delta:
            {
                modifiedChunks = delta.ModifiedChunks;
                beacons = delta.Beacons;
                regions = delta.Regions;

                foreach (var index in component.Chunks.Keys)
                {
                    if (!delta.AllChunks!.Contains(index))
                        component.Chunks.Remove(index);
                }

                break;
            }
            case NavMapState state:
            {
                modifiedChunks = state.Chunks;
                beacons = state.Beacons;
                regions = state.Regions;

                foreach (var index in component.Chunks.Keys)
                {
                    if (!state.Chunks.ContainsKey(index))
                        component.Chunks.Remove(index);
                }

                break;
            }
            default:
                return;
        }

        // Update region data and queue new regions for flooding
        var prevRegionOwners = component.RegionProperties.Keys.ToList();
        var validRegionOwners = new List<NetEntity>();

        component.RegionProperties.Clear();

        foreach (var (regionOwner, regionData) in regions)
        {
            if (!regionData.Seeds.Any())
                continue;

            component.RegionProperties[regionOwner] = regionData;
            validRegionOwners.Add(regionOwner);

            if (component.RegionOverlays.ContainsKey(regionOwner))
                continue;

            if (component.QueuedRegionsToFlood.Contains(regionOwner))
                continue;

            component.QueuedRegionsToFlood.Enqueue(regionOwner);
        }

        // Remove stale region owners
        var regionOwnersToRemove = prevRegionOwners.Except(validRegionOwners);

        foreach (var regionOwnerRemoved in regionOwnersToRemove)
            RemoveNavMapRegion(uid, component, regionOwnerRemoved);

        // Modify chunks
        foreach (var (origin, chunk) in modifiedChunks)
        {
            var newChunk = new NavMapChunk(origin);
            Array.Copy(chunk, newChunk.TileData, chunk.Length);
            component.Chunks[origin] = newChunk;

            // If the affected chunk intersects one or more regions, re-flood them
            if (!component.ChunkToRegionOwnerTable.TryGetValue(origin, out var affectedOwners))
                continue;

            foreach (var affectedOwner in affectedOwners)
            {
                if (!component.QueuedRegionsToFlood.Contains(affectedOwner))
                    component.QueuedRegionsToFlood.Enqueue(affectedOwner);
            }
        }

        // Refresh beacons
        component.Beacons.Clear();
        foreach (var (nuid, beacon) in beacons)
        {
            component.Beacons[nuid] = beacon;
        }
    }
}
