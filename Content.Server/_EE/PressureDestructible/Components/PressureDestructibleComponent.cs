// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;


namespace Content.Server._EE.PressureDestructible.Components;


/// <summary>
///     This is used in making pressure destroy structures.
/// </summary>
[RegisterComponent]
public sealed partial class PressureDestructibleComponent : Component
{
    /// <summary>
    ///     How much pressure could this entity reasonably withstand?
    /// </summary>
    [DataField]
    public float MaxPressureDifferential { get; set; }

    /// <summary>
    ///     How much damage, as a percentage, will the entity take?
    /// </summary>
    [DataField]
    public int Damage { get; set; } = 20;

    [DataField("nextUpdate", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextUpdate { get; set; }

    [DataField]
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromSeconds(5);
}
