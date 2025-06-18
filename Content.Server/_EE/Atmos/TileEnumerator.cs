// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Content.Shared.Atmos;

namespace Content.Server.Atmos;

public struct TileEnumerator
{
    public readonly TileAtmosphere?[] Tiles;
    public int Index = 0;

    public static readonly TileEnumerator Empty = new([]);

    internal TileEnumerator(TileAtmosphere?[] tiles)
    {
        Tiles = tiles;
    }

    public bool MoveNext([NotNullWhen(true)] out TileAtmosphere? tileAtmosphere)
    {
        while (Index < Tiles.Length)
        {
            tileAtmosphere = Tiles[Index++];

            if (tileAtmosphere != null)
                return true;
        }

        tileAtmosphere = null;
        return false;
    }
}
