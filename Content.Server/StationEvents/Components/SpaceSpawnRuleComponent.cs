// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.StationEvents.Events;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.Components;

/// <summary>
/// Component for spawning antags in space around a station.
/// Requires <c>AntagSelectionComponent</c>.
/// </summary>
[RegisterComponent, Access(typeof(SpaceSpawnRule))]
public sealed partial class SpaceSpawnRuleComponent : Component
{
    /// <summary>
    /// Distance that the entity spawns from the station's half AABB radius
    /// </summary>
    [DataField]
    public float SpawnDistance = 20f;

    /// <summary>
    /// Location that was picked.
    /// </summary>
    [DataField]
    public MapCoordinates? Coords;
}
