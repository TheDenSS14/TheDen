// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Inventory;
using Robust.Shared.Prototypes;

namespace Content.Shared.Clothing.Loadouts.Prototypes;


/// <summary>
///     A prototype defining a valid category for <see cref="LoadoutPrototype"/>s to go into.
/// </summary>
[Prototype]
public sealed partial class LoadoutCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public bool Root;

    [DataField]
    public List<ProtoId<LoadoutCategoryPrototype>> SubCategories = new();

    /// <summary>
    ///     Only used for "root" loadouts.
    /// </summary>
    [DataField("order")]
    public int Ordering = 1;

    // DEN: Allow force sorting items into slots per-category
    /// <summary>
    /// Force this loadout category to use a particular slot.
    /// Note: Using the "POCKET" slot will be interpreted as always slotting item into the bag.
    /// Pocket slot will always ignore exclusive!
    /// </summary>
    [DataField]
    public SlotFlags? Slot = null;
}
