// SPDX-FileCopyrightText: 2024 Simon <63975668+Simyon264@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading.Tasks;
using Content.Server.Database;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Network;

namespace Content.Server.Connection.Whitelist.Conditions;

/// <summary>
/// Condition that matches if the player has played for a certain amount of time.
/// </summary>
public sealed partial class ConditionPlaytime : WhitelistCondition
{
    [DataField]
    public int MinimumPlaytime = 0; // In minutes
}
