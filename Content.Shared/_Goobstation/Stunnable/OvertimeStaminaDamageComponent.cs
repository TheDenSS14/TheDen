// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Stunnable;

[RegisterComponent, NetworkedComponent]
public sealed partial class OvertimeStaminaDamageComponent : Component
{
    [DataField] public float Delay = 1f;
    [ViewVariables(VVAccess.ReadWrite)] public float Timer = 1f;

    /// <summary>
    ///     Total amount of stamina damage a person is about to get
    /// </summary>
    [DataField] public float Amount = 10f;

    [ViewVariables(VVAccess.ReadWrite)] public float Damage = 10f;


    /// <summary>
    ///     Divisor. How much damage should we add overtime.
    /// </summary>
    /// <remarks> For example, if the divisor is 5, out entity will get the entire overtime stam damage only after 5 seconds. </remarks>
    [DataField] public float Delta = 5f;
}