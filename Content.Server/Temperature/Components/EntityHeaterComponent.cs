// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Temperature.Systems;
using Content.Shared.Temperature;

namespace Content.Server.Temperature.Components;

/// <summary>
/// Adds thermal energy to entities with <see cref="TemperatureComponent"/> placed on it.
/// </summary>
[RegisterComponent, Access(typeof(EntityHeaterSystem))]
public sealed partial class EntityHeaterComponent : Component
{
    /// <summary>
    /// Power used when heating at the high setting.
    /// Low and medium are 33% and 66% respectively.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Power = 2400f;

    /// <summary>
    /// Current setting of the heater. If it is off or unpowered it won't heat anything.
    /// </summary>
    [DataField]
    public EntityHeaterSetting Setting = EntityHeaterSetting.Off;
}
