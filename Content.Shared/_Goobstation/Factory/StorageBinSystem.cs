// SPDX-FileCopyrightText: 2025 AirFryerBuyOneGetOneFree <airfryerbuyonegetonefree@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Goobstation.Factory.Filters;
using Content.Shared.DeviceLinking;
using Robust.Shared.Containers;

namespace Content.Shared._Goobstation.Factory;

public sealed class StorageBinSystem : EntitySystem
{
    [Dependency] private readonly AutomationFilterSystem _filter = default!;
    [Dependency] private readonly SharedDeviceLinkSystem _device = default!;

    public const string ContainerId = "storagebase";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StorageBinComponent, ContainerIsInsertingAttemptEvent>(OnInsertAttempt);
        SubscribeLocalEvent<StorageBinComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
        SubscribeLocalEvent<StorageBinComponent, EntRemovedFromContainerMessage>(OnEntRemoved);
    }

    private void OnInsertAttempt(Entity<StorageBinComponent> ent, ref ContainerIsInsertingAttemptEvent args)
    {
        if (args.Container.ID != ContainerId)
            return;

        if (_filter.IsBlocked(_filter.GetSlot(ent), args.EntityUid))
            args.Cancel();
    }

    private void OnEntInserted(Entity<StorageBinComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != ContainerId)
            return;

        _device.InvokePort(ent, ent.Comp.InsertedPort);
    }

    private void OnEntRemoved(Entity<StorageBinComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != ContainerId)
            return;

        _device.InvokePort(ent, ent.Comp.RemovedPort);
    }
}
