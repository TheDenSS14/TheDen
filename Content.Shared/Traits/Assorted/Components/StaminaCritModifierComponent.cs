// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
///     This is used for any trait that modifies stamina CritThreshold
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class StaminaCritModifierComponent : Component
{
    /// <summary>
    ///     The amount that an entity's stamina critical threshold will be incremented by.
    /// </summary>
    [DataField]
    public int CritThresholdModifier { get; private set; } = 0;
}
