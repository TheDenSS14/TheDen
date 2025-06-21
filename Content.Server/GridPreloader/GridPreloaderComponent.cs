// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.GridPreloader.Prototypes;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.GridPreloader;

/// <summary>
/// Component storing data about preloaded grids and their location
/// Goes on the map entity
/// </summary>
[RegisterComponent, Access(typeof(GridPreloaderSystem))]
public sealed partial class GridPreloaderComponent : Component
{
    [DataField]
    public Dictionary<ProtoId<PreloadedGridPrototype>, List<EntityUid>> PreloadedGrids = new();
}
