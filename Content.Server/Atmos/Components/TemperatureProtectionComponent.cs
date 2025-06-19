// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Temperature.Systems;

namespace Content.Server.Atmos.Components;

[RegisterComponent]
[Access(typeof(TemperatureSystem))]
public sealed partial class TemperatureProtectionComponent : Component
{
    /// <summary>
    ///     How much to multiply temperature deltas by.
    /// </summary>
    [DataField]
    public float Coefficient = 1.0f;
}

/// <summary>
/// Event raised on an entity with <see cref="TemperatureProtectionComponent"/> to determine the actual value of the coefficient.
/// </summary>
[ByRefEvent]
public record struct GetTemperatureProtectionEvent(float Coefficient);
