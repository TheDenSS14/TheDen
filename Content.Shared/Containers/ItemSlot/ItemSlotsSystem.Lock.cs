// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Lock;

namespace Content.Shared.Containers.ItemSlots;

public sealed partial class ItemSlotsSystem
{
    private void InitializeLock()
    {
        SubscribeLocalEvent<ItemSlotsLockComponent, MapInitEvent>(OnLockMapInit);
        SubscribeLocalEvent<ItemSlotsLockComponent, LockToggledEvent>(OnLockToggled);
    }

    private void OnLockMapInit(Entity<ItemSlotsLockComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp(ent.Owner, out LockComponent? lockComp))
            return;

        UpdateLocks(ent, lockComp.Locked);
    }

    private void OnLockToggled(Entity<ItemSlotsLockComponent> ent, ref LockToggledEvent args)
    {
        UpdateLocks(ent, args.Locked);
    }

    private void UpdateLocks(Entity<ItemSlotsLockComponent> ent, bool value)
    {
        foreach (var slot in ent.Comp.Slots)
        {
            if (!TryGetSlot(ent.Owner, slot, out var itemSlot))
                continue;

            SetLock(ent.Owner, itemSlot, value);
        }
    }
}
