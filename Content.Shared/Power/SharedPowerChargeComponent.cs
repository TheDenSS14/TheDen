// SPDX-FileCopyrightText: 2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Power;

/// <summary>
/// Component for a powered machine that slowly powers on and off over a period of time.
/// </summary>
public abstract partial class SharedPowerChargeComponent : Component
{
    /// <summary>
    /// The title used for the default charged machine window if used
    /// </summary>
    [DataField]
    public LocId WindowTitle { get; set; } = string.Empty;

}
