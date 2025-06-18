// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// An objective that is set to complete by code in another system.
/// Use <see cref="CodeConditionSystem"/> to check and set this.
/// </summary>
[RegisterComponent, Access(typeof(CodeConditionSystem))]
public sealed partial class CodeConditionComponent : Component
{
    /// <summary>
    /// Whether the objective is complete or not.
    /// </summary>
    [DataField]
    public bool Completed;
}
