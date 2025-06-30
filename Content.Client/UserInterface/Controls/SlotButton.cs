// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using static Content.Client.Inventory.ClientInventorySystem;

namespace Content.Client.UserInterface.Controls
{
    public sealed class SlotButton : SlotControl
    {
        public SlotButton() { }

        public SlotButton(SlotData slotData)
        {
            ButtonTexturePath = slotData.TextureName;
            FullButtonTexturePath = slotData.FullTextureName;
            Blocked = slotData.Blocked;
            Highlight = slotData.Highlighted;
            StorageTexturePath = "Slots/back";
            SlotName = slotData.SlotName;
        }
    }
}
