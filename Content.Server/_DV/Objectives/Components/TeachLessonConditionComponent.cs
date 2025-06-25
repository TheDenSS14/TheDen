// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DV.Objectives.Systems;
using Content.Server.Objectives.Components;

namespace Content.Server._DV.Objectives.Components;

/// <summary>
/// Requires that a target dies once and only once.
/// Depends on <see cref="TargetObjectiveComponent"/> to function.
/// </summary>
[RegisterComponent, Access(typeof(TeachLessonConditionSystem))]
public sealed partial class TeachLessonConditionComponent : Component
{
    /// <summary>
    ///     How close the assassin must be to the person "Being given a lesson", to ensure that the kill is reasonably
    ///     something that could be the assassin's doing. This way the objective isn't resolved by the target getting killed
    ///     by a space tick while on expedition.
    /// </summary>
    [DataField]
    public float MaxDistance = 30f;
}
