// SPDX-FileCopyrightText: 2023 JJ <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
///     Requires that the player ensures glimmer remain above a specific amount.
/// </summary>
[RegisterComponent, Access(typeof(RaiseGlimmerConditionSystem))]
public sealed partial class RaiseGlimmerConditionComponent : Component
{
    [DataField]
    public float Target;
}
