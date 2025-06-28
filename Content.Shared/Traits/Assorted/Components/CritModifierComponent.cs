// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 RedFoxIV <38788538+RedFoxIV@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
///     This is used for any trait that modifies CritThreshold
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CritModifierComponent : Component
{
    /// <summary>
    ///     The amount that an entity's critical threshold will be incremented by.
    /// </summary>
    [DataField]
    public int CritThresholdModifier { get; private set; } = 0;
}