// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Goobstation.Spawn.Components;

/// <summary>
///     Component-marker for unique entity
/// </summary>
[RegisterComponent]
public sealed partial class UniqueEntityMarkerComponent : Component
{
    /// <summary>
    ///     Marker name that would be used in check
    /// </summary>
    [DataField]
    public string? MarkerName;

    /// <summary>
    ///     If true - marker will work on grids with StationDataComponent
    ///     If false - marker will work globally
    /// </summary>
    [DataField]
    public bool StationOnly = true;
}
