// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Ghost.Roles.Raffles;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ghost.Roles.Raffles;

/// <summary>
/// Allows specifying the settings for a ghost role raffle as a prototype.
/// </summary>
[Prototype("ghostRoleRaffleSettings")]
public sealed class GhostRoleRaffleSettingsPrototype : IPrototype
{
    /// <inheritdoc />
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The settings for a ghost role raffle.
    /// </summary>
    /// <seealso cref="GhostRoleRaffleSettings"/>
    [DataField(required: true)]
    public GhostRoleRaffleSettings Settings { get; private set; } = new();
}
