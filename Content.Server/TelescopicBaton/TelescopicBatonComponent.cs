// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.TelescopicBaton;

[RegisterComponent]
public sealed partial class TelescopicBatonComponent : Component
{
    [DataField]
    public bool CanKnockDown;

    /// <summary>
    ///     The amount of time during which the baton will be able to knockdown someone after activating it.
    /// </summary>
    [DataField]
    public TimeSpan AttackTimeframe = TimeSpan.FromSeconds(1.5f);

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan TimeframeAccumulator = TimeSpan.FromSeconds(0);
}
