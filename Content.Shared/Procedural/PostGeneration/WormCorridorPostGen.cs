// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Maps;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.PostGeneration;

// Ime a worm
/// <summary>
/// Generates worm corridors.
/// </summary>
public sealed partial class WormCorridorPostGen : IPostDunGen
{
    [DataField]
    public int PathLimit = 2048;

    /// <summary>
    /// How many times to run the worm
    /// </summary>
    [DataField]
    public int Count = 20;

    /// <summary>
    /// How long to make each worm
    /// </summary>
    [DataField]
    public int Length = 20;

    /// <summary>
    /// Maximum amount the angle can change in a single step.
    /// </summary>
    [DataField]
    public Angle MaxAngleChange = Angle.FromDegrees(45);

    [DataField]
    public ProtoId<ContentTileDefinition> Tile = "FloorSteel";

    /// <summary>
    /// How wide to make the corridor.
    /// </summary>
    [DataField]
    public float Width = 3f;
}
