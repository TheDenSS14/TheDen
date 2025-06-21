// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Station.Systems;
using Robust.Shared.Utility;

namespace Content.Server.Station.Components;

/// <summary>
/// Loads a surface map on mapinit.
/// </summary>
[RegisterComponent, Access(typeof(StationSurfaceSystem))]
public sealed partial class StationSurfaceComponent : Component
{
    /// <summary>
    /// Path to the map to load.
    /// </summary>
    [DataField(required: true)]
    public ResPath? MapPath;

    /// <summary>
    /// The map that was loaded.
    /// </summary>
    [DataField]
    public EntityUid? Map;
}
