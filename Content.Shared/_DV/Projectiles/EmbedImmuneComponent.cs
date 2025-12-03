// SPDX-FileCopyrightText: 2025 William Lemon
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared._DV.Projectiles;

/// <summary>
/// Indicates that the entity cannot be embedded with select projectiles.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EmbedImmuneComponent : Component
{
    /// <summary>
    /// A list of projectiles that this entity is immune to being embedded by.
    /// If null, the entity is immune to all projectiles.
    /// </summary>
    [DataField(required: true)]
    public EntityWhitelist ImmuneTo = default!;
}
