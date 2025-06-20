// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Humanoid;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.Components;

/// <summary>
/// This is used for a clothing item that hides an appearance layer.
/// The entity's HumanoidAppearance component must have the corresponding hideLayerOnEquip value.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HideLayerClothingComponent : Component
{
    /// <summary>
    /// The appearance layer to hide.
    /// </summary>
    [DataField]
    public HashSet<HumanoidVisualLayers> Slots = new();

    /// <summary>
    /// The clothing layers to hide.
    /// </summary>
    [DataField]
    public HashSet<string>? ClothingSlots = new();

    /// <summary>
    /// If true, the layer will only hide when the item is in a toggled state (e.g. masks)
    /// </summary>
    [DataField]
    public bool HideOnToggle = false;

    /// <summary>
    /// If true, the layer will always be hidden even if the layer
    /// is not present in the equipee's HumanoidAppearanceComponent.HideLayerOnEquip field.
    /// </summary>
    [DataField]
    public bool Force = false;
}
