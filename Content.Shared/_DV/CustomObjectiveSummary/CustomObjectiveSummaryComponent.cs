// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._DV.CustomObjectiveSummary;

/// <summary>
///     Put on a players mind if the wrote a custom summary for their objectives.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CustomObjectiveSummaryComponent : Component
{
    /// <summary>
    ///     What the player wrote as their summary!
    /// </summary>
    [DataField, AutoNetworkedField]
    public string ObjectiveSummary = "";
}
