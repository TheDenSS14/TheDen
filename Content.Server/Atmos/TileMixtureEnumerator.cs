// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Content.Shared.Atmos;

namespace Content.Server.Atmos;

public struct TileMixtureEnumerator
{
    public readonly TileAtmosphere?[] Tiles;
    public int Index = 0;

    public static readonly TileMixtureEnumerator Empty = new(Array.Empty<TileAtmosphere>());

    internal TileMixtureEnumerator(TileAtmosphere?[] tiles)
    {
        Tiles = tiles;
    }

    public bool MoveNext([NotNullWhen(true)] out GasMixture? mix)
    {
        while (Index < Tiles.Length)
        {
            mix = Tiles[Index++]?.Air;
            if (mix != null)
                return true;
        }

        mix = null;
        return false;
    }
}
