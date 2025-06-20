// SPDX-FileCopyrightText: 2024 Simon <63975668+Simyon264@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Robust.Shared.Network;

namespace Content.Server.Connection.Whitelist;

/// <summary>
/// This class is used to determine if a player should be allowed to join the server.
/// It is used in <see cref="PlayerConnectionWhitelistPrototype"/>
/// </summary>
[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class WhitelistCondition
{
    /// <summary>
    /// What action should be taken if this condition is met?
    /// Defaults to <see cref="ConditionAction.Next"/>.
    /// </summary>
    [DataField]
    public ConditionAction Action { get; set; } = ConditionAction.Next;
}

/// <summary>
/// Determines what action should be taken if a condition is met.
/// </summary>
public enum ConditionAction
{
    /// <summary>
    /// The player is allowed to join, and the next conditions will be skipped.
    /// </summary>
    Allow,
    /// <summary>
    /// The player is denied to join, and the next conditions will be skipped.
    /// </summary>
    Deny,
    /// <summary>
    /// The next condition should be checked.
    /// </summary>
    Next
}
