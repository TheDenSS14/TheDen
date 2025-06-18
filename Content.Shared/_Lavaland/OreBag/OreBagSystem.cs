// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Salvage.Components;
using Content.Shared.Storage;
using Content.Shared.Interaction;
using Content.Shared.Materials;
using Robust.Shared.Timing;

namespace Content.Shared._Lavaland.OreBag;

public sealed class OreBagSystem : EntitySystem
{
    [Dependency] private readonly SharedMaterialStorageSystem _materialStorage = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<OreBagComponent, AfterInteractEvent>(OnAfterInteract);
    }

    private void OnAfterInteract(EntityUid uid, OreBagComponent component, AfterInteractEvent args)
    {
        if (!args.CanReach
          || args.Target == null
          || !HasComp<MiningPointsLatheComponent>(args.Target)
          || !_timing.IsFirstTimePredicted)
            return;

        if (!TryComp<StorageComponent>(uid, out var storage))
            return;

        var validEntities = new List<EntityUid>();

        foreach (var entity in storage.Container.ContainedEntities)
            if (HasComp<MaterialComponent>(entity))
                validEntities.Add(entity);

        foreach (var entity in validEntities)
            _materialStorage.TryInsertMaterialEntity(args.User, entity, args.Target.Value);
    }
}
