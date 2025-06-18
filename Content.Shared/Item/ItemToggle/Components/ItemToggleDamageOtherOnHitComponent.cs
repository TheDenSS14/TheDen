// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage.Systems;
using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Item.ItemToggle.Components;

/// <summary>
///   Handles changes to DamageOtherOnHitComponent when the item is toggled.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ItemToggleDamageOtherOnHitComponent : Component
{
    /// <summary>
    ///   The stamina cost of throwing this entity when activated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float? ActivatedStaminaCost = null;

    /// <summary>
    ///   The stamina cost of throwing this entity when deactivated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float? DeactivatedStaminaCost = null;

    /// <summary>
    ///     Damage done by this item when activated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier? ActivatedDamage = null;

    /// <summary>
    ///     Damage done by this item when deactivated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier? DeactivatedDamage = null;

    /// <summary>
    ///   The noise this item makes when hitting something with it on.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? ActivatedSoundHit;

    /// <summary>
    ///   The noise this item makes when hitting something with it off.
    /// </summary>
    public SoundSpecifier? DeactivatedSoundHit;

    /// <summary>
    ///  The noise this item makes when hitting something with it off and it does no damage.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier ActivatedSoundNoDamage { get; set; } = new SoundCollectionSpecifier("WeakHit");

    /// <summary>
    ///   The noise this item makes when hitting something with it off and it does no damage.
    /// </summary>
    public SoundSpecifier? DeactivatedSoundNoDamage;
}
