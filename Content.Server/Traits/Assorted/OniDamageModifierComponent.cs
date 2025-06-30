// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;

namespace Content.Server.Traits.Assorted;

/// <summary>
///     This is used for traits that modify Oni damage modifiers.
/// </summary>
[RegisterComponent]
public sealed partial class OniDamageModifierComponent : Component
{
    /// <summary>
    ///     Which damage modifiers to override.
    /// </summary>
    [DataField("modifiers", required: true)]
    public DamageModifierSet MeleeModifierReplacers = default!;
}
