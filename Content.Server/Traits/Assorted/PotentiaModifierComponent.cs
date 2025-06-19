// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Traits.Assorted;

/// <summary>
///     This is used for traits that modify the outcome of Potentia Rolls
/// </summary>
[RegisterComponent]
public sealed partial class PotentiaModifierComponent : Component
{
    /// <summary>
    ///     When rolling for psionic powers, increase the potentia gains by a flat amount.
    /// </summary>
    [DataField]
    public float PotentiaFlatModifier = 0;

    /// <summary>
    ///     When rolling for psionic powers, multiply the potentia gains by a specific factor.
    /// </summary>
    [DataField]
    public float PotentiaMultiplier = 1;
}
