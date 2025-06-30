// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Damage.Events;

/// <summary>
///   Raised on a throwing weapon to calculate potential damage bonuses or decreases.
/// </summary>
[ByRefEvent]
public record struct GetThrowingDamageEvent(EntityUid Weapon, DamageSpecifier Damage, List<DamageModifierSet> Modifiers, EntityUid? Target, EntityUid? User);

/// <summary>
///   Raised on a throwing weapon when DamageOtherOnHit has been successfully initialized.
/// </summary>
public record struct DamageOtherOnHitStartupEvent(Entity<DamageOtherOnHitComponent> Weapon);

/// <summary>
///   Raised on a throwing weapon when ItemToggleDamageOtherOnHit has been successfully initialized.
/// </summary>
public record struct ItemToggleDamageOtherOnHitStartupEvent(Entity<ItemToggleDamageOtherOnHitComponent> Weapon);
