// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Inventory;

namespace Content.Shared._DEN.Body;

/// <summary>
///     Whether or not this entity can support standing up when they have less than the required amount of legs.
///     Cancel this event if the entity should be able to stand anyway.
/// </summary>

public sealed class CannotSupportStandingEvent : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;
    public int LegCount;
    public bool Forced = false;

    public CannotSupportStandingEvent(int legCount)
    {
        LegCount = legCount;
    }
}
