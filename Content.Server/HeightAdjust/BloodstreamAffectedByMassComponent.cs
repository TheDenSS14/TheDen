// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Components;

namespace Content.Server.HeightAdjust;

/// <summary>
///     When applied to a humanoid or any mob, adjusts their blood level based on the mass contest between them
///     and an average humanoid.
///     <br/>
///     The formula for the resulting bloodstream volume is <code>V = BloodMaxVolume * MassContest^Power</code>
///     clamped between the specified Min and Max values.
/// </summary>
[RegisterComponent]
public sealed partial class BloodstreamAffectedByMassComponent : Component
{
    /// <summary>
    ///     Minimum and maximum resulting volume factors. A minimum value of 0.5 means that the resulting volume will be at least 50% of the original.
    /// </summary>
    [DataField]
    public float Min = 1 / 3f, Max = 3f;

    /// <summary>
    ///     The power to which the outcome of the mass contest will be risen.
    /// </summary>
    [DataField]
    public float Power = 1f;
}
