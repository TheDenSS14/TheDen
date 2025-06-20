// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Chemistry;

/// <summary>
/// A component that works in tandem with <see cref="RottingComponent"/>. When this entity is rotten,
/// the solution with the given name will be replaced with a given other solution.
/// For example, rotting corpses may build up gastrotoxin.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ReplaceSolutionWhenRottenComponent : BaseReplaceSolutionIntervalComponent
{ }
