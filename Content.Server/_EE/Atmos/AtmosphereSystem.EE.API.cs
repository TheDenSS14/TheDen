// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Content.Server.Atmos.Components;
using Content.Shared.Atmos;


namespace Content.Server.Atmos.EntitySystems;

/// <summary>
/// This handles...
/// </summary>
public partial class AtmosphereSystem
{
    public TileAtmosphere? GetTileAtmosphere(Entity<GridAtmosphereComponent?> grid, Vector2i tile)
    {
        if (!_atmosQuery.Resolve(grid, ref grid.Comp, false))
            return null;

        return grid.Comp.Tiles.TryGetValue(tile, out var atmosTile) ? atmosTile : null;
    }

    public bool TryGetTileAtmosphere(Entity<GridAtmosphereComponent?> grid, Vector2i tile, [NotNullWhen(true)] out TileAtmosphere? tileAtmosphere)
    {
        if (!_atmosQuery.Resolve(grid, ref grid.Comp, false))
        {
            tileAtmosphere = null;
            return false;
        }

        var success = grid.Comp.Tiles.TryGetValue(tile, out var atmosTile);
        tileAtmosphere = success ? atmosTile : null;
        return success;
    }

    public TileEnumerator GetAdjacentTileAtmospheres(Entity<GridAtmosphereComponent?> grid, Vector2i tile, bool includeBlocked = false, bool excite = false)
    {
        if (!_atmosQuery.Resolve(grid, ref grid.Comp, false))
            return TileEnumerator.Empty;

        return !grid.Comp.Tiles.TryGetValue(tile, out var atmosTile)
            ? TileEnumerator.Empty
            : new(atmosTile.AdjacentTiles);
    }
}
