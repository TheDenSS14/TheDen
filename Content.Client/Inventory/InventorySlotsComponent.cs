// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Client.Inventory;

/// <summary>
/// A character UI which shows items the user has equipped within his inventory
/// </summary>
[RegisterComponent]
[Access(typeof(ClientInventorySystem))]
public sealed partial class InventorySlotsComponent : Component
{
    [ViewVariables]
    public readonly Dictionary<string, ClientInventorySystem.SlotData> SlotData = new();

    /// <summary>
    ///     Data about the current layers that have been added to the players sprite due to the items in each equipment slot.
    /// </summary>
    [ViewVariables]
    [Access(typeof(ClientInventorySystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public readonly Dictionary<string, HashSet<string>> VisualLayerKeys = new();

    /// <summary>
    ///     The slots whose associated visual layers are hidden.
    /// </summary>
    [ViewVariables]
    [Access(typeof(ClientInventorySystem), Other = AccessPermissions.ReadWriteExecute)]
    public readonly HashSet<string> HiddenSlots = new();
}
