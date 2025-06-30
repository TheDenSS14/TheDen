// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Stunnable;

[RegisterComponent, NetworkedComponent]
public sealed partial class StaminaDamageResistanceComponent : Component
{
    /// <summary>
    ///     1 - no reduction, 0 - full reduction
    /// </summary>
    [DataField] public float Coefficient = 1;
}
