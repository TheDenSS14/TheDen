// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared._DEN.Movement.Components;

/// <summary>
///     This component should be added to entities who should not fall over immediately
///     when they otherwise should. For example, a character who has been missing a leg
///     for a long time, so they can stand for a short time even after putting their mobility aid down.
/// </summary>
[RegisterComponent]
public sealed partial class AdaptedBalanceComponent : Component
{
    /// <summary>
    ///     How long this entity can stand without additional support.
    /// </summary>
    [DataField]
    public TimeSpan BalanceDuration = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     How many legs this entity must have in order to keep their balance.
    /// </summary>
    [DataField]
    public int MinimumLegs = 1;
}

/// <summary>
///     This component is applied to entities who should remain standing.
/// </summary>
[RegisterComponent]
public sealed partial class ActiveAdaptedBalanceComponent : Component
{
    public TimeSpan EndTime;
}
