// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Inventory.Controls;

public sealed class ItemSlotButtonContainer : ItemSlotUIContainer<SlotControl>
{
    private readonly InventoryUIController _inventoryController;
    private string _slotGroup = "";

    public string SlotGroup
    {
        get => _slotGroup;
        set
        {
            _inventoryController.RemoveSlotGroup(SlotGroup);
            _slotGroup = value;
            _inventoryController.RegisterSlotGroupContainer(this);
        }
    }

    public ItemSlotButtonContainer()
    {
        _inventoryController = UserInterfaceManager.GetUIController<InventoryUIController>();
    }

    ~ItemSlotButtonContainer()
    {
        _inventoryController.RemoveSlotGroup(SlotGroup);
    }
}
