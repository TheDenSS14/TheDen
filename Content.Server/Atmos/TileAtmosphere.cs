// SPDX-FileCopyrightText: 2020 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2020 DTanxxx <55208219+DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2020 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 2020 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2022 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Cam <Nop>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using System.Numerics;

namespace Content.Server.Atmos;

/// <summary>
///     Internal Atmos class that stores data about the atmosphere in a grid.
///     You shouldn't use this directly, use <see cref="AtmosphereSystem"/> instead.
/// </summary>
[Access(typeof(AtmosphereSystem), typeof(GasTileOverlaySystem), typeof(AtmosDebugOverlaySystem))]
public sealed class TileAtmosphere : IGasMixtureHolder
{
    [ViewVariables]
    public int ArchivedCycle;

    [ViewVariables]
    public int CurrentCycle;

    [ViewVariables]
    public float Temperature { get; set; } = Atmospherics.T20C;

    [ViewVariables]
    public float TemperatureArchived { get; set; } = Atmospherics.T20C;


    [ViewVariables(VVAccess.ReadWrite)]
    public float HeatCapacity { get; set; } = Atmospherics.MinimumHeatCapacity;

    [ViewVariables]
    public float ThermalConductivity { get; set; } = 0.05f;

    [ViewVariables]
    public bool Excited { get; set; }

    /// <summary>
    ///     Whether this tile should be considered space.
    /// </summary>
    [ViewVariables]
    public bool Space { get; set; }

    /// <summary>
    ///     Adjacent tiles in the same order as <see cref="AtmosDirection"/>. (NSEW)
    /// </summary>
    [ViewVariables]
    public readonly TileAtmosphere?[] AdjacentTiles = new TileAtmosphere[Atmospherics.Directions];

    /// <summary>
    /// Neighbouring tiles to which air can flow. This is a combination of this tile's unblocked direction, and the
    /// unblocked directions on adjacent tiles.
    /// </summary>
    [ViewVariables]
    public AtmosDirection AdjacentBits = AtmosDirection.Invalid;

    [ViewVariables, Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)]
    public MonstermosInfo MonstermosInfo;

    [ViewVariables]
    public Hotspot Hotspot;

    // For debug purposes.
    [ViewVariables]
    public Vector2 LastPressureDirection;

    [ViewVariables]
    [Access(typeof(AtmosphereSystem))]
    public EntityUid GridIndex { get; set; }

    [ViewVariables]
    public Vector2i GridIndices;

    [ViewVariables]
    public ExcitedGroup? ExcitedGroup { get; set; }

    /// <summary>
    /// The air in this tile. If null, this tile is completely air-blocked.
    /// This can be immutable if the tile is spaced.
    /// </summary>
    [ViewVariables]
    [Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
    public GasMixture? Air { get; set; }

    [ViewVariables]
    public GasMixture? AirArchived { get; set; }

    [DataField("lastShare")]
    public float LastShare;

    GasMixture IGasMixtureHolder.Air
    {
        get => Air ?? new GasMixture(Atmospherics.CellVolume){ Temperature = Temperature };
        set => Air = value;
    }

    [ViewVariables]
    public float MaxFireTemperatureSustained { get; set; }

    /// <summary>
    /// If true, then this tile is directly exposed to the map's atmosphere, either because the grid has no tile at
    /// this position, or because the tile type is not airtight.
    /// </summary>
    [ViewVariables]
    public bool MapAtmosphere;

    /// <summary>
    /// If true, this tile does not actually exist on the grid, it only exists to represent the map's atmosphere for
    /// adjacent grid tiles.
    /// </summary>
    [ViewVariables]
    public bool NoGridTile;

    /// <summary>
    /// If true, this tile is queued for processing in <see cref="GridAtmosphereComponent.PossiblyDisconnectedTiles"/>
    /// </summary>
    [ViewVariables]
    public bool TrimQueued;

    /// <summary>
    /// Cached information about airtight entities on this tile. This gets updated anytime a tile gets invalidated
    /// (i.e., gets added to <see cref="GridAtmosphereComponent.InvalidatedCoords"/>).
    /// </summary>
    public AtmosphereSystem.AirtightData AirtightData;

    public TileAtmosphere(EntityUid gridIndex, Vector2i gridIndices, GasMixture? mixture = null, bool immutable = false, bool space = false)
    {
        GridIndex = gridIndex;
        GridIndices = gridIndices;
        Air = mixture;
        AirArchived = Air?.Clone();
        Space = space;

        if(immutable)
            Air?.MarkImmutable();
    }

    public TileAtmosphere() { }

    public TileAtmosphere(TileAtmosphere other)
    {
        GridIndex = other.GridIndex;
        GridIndices = other.GridIndices;
        Space = other.Space;
        NoGridTile = other.NoGridTile;
        MapAtmosphere = other.MapAtmosphere;
        Air = other.Air?.Clone();
        AirArchived = Air?.Clone();
    }
}
