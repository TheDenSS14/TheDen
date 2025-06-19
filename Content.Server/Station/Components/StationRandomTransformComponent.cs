// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Station.Systems;

namespace Content.Server.Station.Components;

/// <summary>
/// Stores station parameters that can be randomized by the roundstart
/// </summary>
[RegisterComponent, Access(typeof(StationSystem))]
public sealed partial class StationRandomTransformComponent : Component
{
    [DataField]
    public float? MaxStationOffset = 100.0f;

    [DataField]
    public bool EnableStationRotation = false;
}
