// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 PrPleGoo <prplegoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Inventory.Events;

public sealed class RefreshEquipmentHudEvent<T> : EntityEventArgs, IInventoryRelayEvent where T : IComponent
{
    public SlotFlags TargetSlots { get; init; }
    public bool Active = false;
    public List<T> Components = new();

    public RefreshEquipmentHudEvent(SlotFlags targetSlots)
    {
        TargetSlots = targetSlots;
    }
}
