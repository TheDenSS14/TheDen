// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Maps;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Connects room entrances via corridor segments.
/// </summary>
public sealed partial class CorridorPostGen : IPostDunGen
{
    /// <summary>
    /// How far we're allowed to generate a corridor before calling it.
    /// </summary>
    /// <remarks>
    /// Given the heavy weightings this needs to be fairly large for larger dungeons.
    /// </remarks>
    [DataField]
    public int PathLimit = 2048;

    [DataField]
    public ProtoId<ContentTileDefinition> Tile = "FloorSteel";

    /// <summary>
    /// How wide to make the corridor.
    /// </summary>
    [DataField]
    public float Width = 3f;
}
