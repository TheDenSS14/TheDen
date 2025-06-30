// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Traits.Assorted;

/// <summary>
///     This is used for the Blood Deficiency trait.
/// </summary>
[RegisterComponent]
public sealed partial class BloodDeficiencyComponent : Component
{
    // <summary>
    ///     How much percentage of max blood volume should be removed in each update interval?
    // </summary>
    [DataField(required: true)]
    public float BloodLossPercentage;

    /// <summary>
    ///     Whether the effects of this trait should be active.
    /// </summary>
    [DataField]
    public bool Active = true;
}
