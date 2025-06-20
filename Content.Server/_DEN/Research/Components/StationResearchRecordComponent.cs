// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._DEN.Research.Components;


/// <summary>
/// This is used for keeping track of soft cap multiplier.
/// </summary>
[RegisterComponent]
public sealed partial class StationResearchRecordComponent : Component
{
    [DataField]
    public float SoftCapMultiplier = 1f;
}
