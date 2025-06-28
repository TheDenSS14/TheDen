// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared._DEN.Movement.Components;

public abstract partial class LegSupportComponent : Component
{
    /// <summary>
    ///     Percentage to the penalty to walking speed incurred by having less than the required number of legs.
    ///     In other words, if you're missing legs, you can walk faster using this.
    /// </summary>
    [DataField]
    public float WalkSpeedPenaltyReduction = 0.0f;

    /// <summary>
    ///     Percentage to the penalty to sprinting speed incurred by having less than the required number of legs.
    ///     In other words, if you're missing legs, you can sprint faster using this.
    /// </summary>
    [DataField]
    public float SprintSpeedPenaltyReduction = 0.0f;
}

/// <summary>
///     Items with this component will reduce the speed penalty for missing legs when held in your hand.
///     For example, if having one leg reduces your speed by 50%, holding an item with this component
///     may soften this reduction by half, so you only have a 25% speed penalty.
/// </summary>
[RegisterComponent]
public sealed partial class HeldLegSupportComponent : LegSupportComponent
{ }
