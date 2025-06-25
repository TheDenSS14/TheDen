// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Spreader;

/// <summary>
/// Raised when trying to spread to neighboring tiles.
/// If the spread is no longer able to happen you MUST cancel this event!
/// </summary>
[ByRefEvent]
public record struct SpreadNeighborsEvent
{
    public ValueList<(MapGridComponent Grid, TileRef Tile)> NeighborFreeTiles;
    public ValueList<EntityUid> Neighbors;

    /// <summary>
    /// How many updates allowed are remaining.
    /// Subscribers can handle as they wish.
    /// </summary>
    public int Updates;
}
