// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Server.Ghost.Roles.Raffles;

/// <summary>
/// Allows getting a <see cref="IGhostRoleRaffleDecider"/> as prototype.
/// </summary>
[Prototype("ghostRoleRaffleDecider")]
public sealed class GhostRoleRaffleDeciderPrototype : IPrototype
{
    /// <inheritdoc />
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The <see cref="IGhostRoleRaffleDecider"/> instance that chooses the winner of a raffle.
    /// </summary>
    [DataField("decider", required: true)]
    public IGhostRoleRaffleDecider Decider { get; private set; } = new RngGhostRoleRaffleDecider();
}
