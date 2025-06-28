// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;

namespace Content.Server.Abilities.Boxer;

/// <summary>
/// Added to the boxer on spawn.
/// </summary>
[RegisterComponent]
public sealed partial class BoxerComponent : Component
{
    [DataField("modifiers", required: true)]
    public DamageModifierSet UnarmedModifiers = default!;

    [DataField("rangeBonus")]
    public float RangeBonus = 1.0f;

    /// <summary>
    /// Damage modifier with boxing glove stam damage.
    /// </summary>
    [DataField("boxingGlovesModifier")]
    public float BoxingGlovesModifier = 1.75f;
}
