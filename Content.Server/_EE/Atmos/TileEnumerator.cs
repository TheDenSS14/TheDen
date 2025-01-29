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

    public bool MoveNext([NotNullWhen(true)] out TileAtmosphere? tileAtmosphere, [NotNullWhen(true)] out int? index)
    {
        while (Index < Tiles.Length)
        {
            tileAtmosphere = Tiles[Index++];
            index = Index;

            if (tileAtmosphere != null)
                return true;
        }

        index = null;
        tileAtmosphere = null;
        return false;
    }
}
