// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Power.Components;

namespace Content.Shared.Power.EntitySystems;

public sealed class ItemSlotRequiresPowerSystem : EntitySystem
{
    [Dependency] private readonly SharedPowerReceiverSystem _receiver = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ItemSlotRequiresPowerComponent, ItemSlotInsertAttemptEvent>(OnInsertAttempt);
    }

    private void OnInsertAttempt(Entity<ItemSlotRequiresPowerComponent> ent, ref ItemSlotInsertAttemptEvent args)
    {
        if (!_receiver.IsPowered(ent.Owner))
        {
            args.Cancelled = true;
        }
    }
}
