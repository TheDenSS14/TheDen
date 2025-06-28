// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Procedural;

public sealed class Dungeon
{
    public readonly List<DungeonRoom> Rooms;

    /// <summary>
    /// Hashset of the tiles across all rooms.
    /// </summary>
    public readonly HashSet<Vector2i> RoomTiles = new();

    public readonly HashSet<Vector2i> RoomExteriorTiles = new();

    public readonly HashSet<Vector2i> CorridorTiles = new();

    public readonly HashSet<Vector2i> CorridorExteriorTiles = new();

    public readonly HashSet<Vector2i> Entrances = new();

    public Dungeon()
    {
        Rooms = new List<DungeonRoom>();
    }

    public Dungeon(List<DungeonRoom> rooms)
    {
        Rooms = rooms;

        foreach (var room in Rooms)
        {
            Entrances.UnionWith(room.Entrances);
        }
    }
}
