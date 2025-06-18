// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
namespace Content.Server.Traits.Assorted;

/// <summary>
///   This is used for the Hemophilia trait.
/// </summary>
[RegisterComponent]
public sealed partial class HemophiliaComponent : Component
{
    // <summary>
    //   What the BleedReductionAmount should be multiplied by.
    // </summary>
    [DataField(required: true)]
    public float BleedReductionModifier = 1f;

    /// <summary>
    ///   The damage increase from this trait.
    /// </summary>
    [DataField(required: true)]
    public DamageModifierSet DamageModifiers = default!;
}
