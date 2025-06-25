// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chemistry.EntitySystems;
using Content.Client.Chemistry.UI;

namespace Content.Client.Chemistry.Components;

/// <summary>
/// Exposes a solution container's contents via a basic item status control.
/// </summary>
/// <remarks>
/// Shows the solution volume, max volume, and transfer amount.
/// </remarks>
/// <seealso cref="SolutionItemStatusSystem"/>
/// <seealso cref="SolutionStatusControl"/>
[RegisterComponent]
public sealed partial class SolutionItemStatusComponent : Component
{
    /// <summary>
    /// The ID of the solution that will be shown on the item status control.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string Solution = "default";
}
