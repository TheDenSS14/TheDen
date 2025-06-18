// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Robust.Shared.GameStates;

namespace Content.Shared.Item.ItemToggle.Components;

/// <summary>
///   Handles the changes to the embed passive damage when toggled.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ItemToggleEmbedPassiveDamageComponent : Component
{
    /// <summary>
    ///   Damage per interval dealt to the entity every interval when activated.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public DamageSpecifier? ActivatedDamage = null;

    /// <summary>
    ///   Damage per interval dealt to the entity every interval when deactivated.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public DamageSpecifier? DeactivatedDamage = null;
}
