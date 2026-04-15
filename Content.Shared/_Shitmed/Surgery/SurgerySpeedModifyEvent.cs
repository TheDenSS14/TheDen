// SPDX-FileCopyrightText: 2025 Sir Warock
// SPDX-FileCopyrightText: 2025 pathetic meowmeow
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Inventory;

/// <summary>
///     Raised on an entity when a surgery is about to be performed, in case a system wants to modify the speed, such as surgical gloves.
/// </summary>
[ByRefEvent]
public record struct SurgerySpeedModifyEvent() : IInventoryRelayEvent // DEN - Rewrote Event
{
    public float Multiplier = 1f; // DEN

    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET; // DEN
}
