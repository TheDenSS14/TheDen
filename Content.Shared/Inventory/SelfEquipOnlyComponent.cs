// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Inventory;

/// <summary>
/// This is used for an item that can only be equipped/unequipped by the user.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SelfEquipOnlySystem))]
public sealed partial class SelfEquipOnlyComponent : Component
{
    /// <summary>
    /// Whether or not the self-equip only condition requires the person to be conscious.
    /// </summary>
    [DataField]
    public bool UnequipRequireConscious = true;
}
