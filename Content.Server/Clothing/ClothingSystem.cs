// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Clothing.Components;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Inventory.Events;
using Robust.Shared.Serialization.Manager;

namespace Content.Server.Clothing;

public sealed class ServerClothingSystem : ClothingSystem
{
    [Dependency] private readonly IComponentFactory _componentFactory = default!;
    [Dependency] private readonly ISerializationManager _serializationManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ClothingComponent, DidEquipEvent>(OnClothingEquipped);
        SubscribeLocalEvent<ClothingComponent, DidUnequipEvent>(OnClothingUnequipped);
    }

    private void OnClothingEquipped(EntityUid uid, ClothingComponent clothingComponent, DidEquipEvent args)
    {
        // Yes, this is using the trait system functions. I'm not going to write an entire third function library to do this.
        // They're generic for a reason.
        foreach (var function in clothingComponent.OnEquipFunctions)
            function.OnPlayerSpawn(uid, _componentFactory, EntityManager, _serializationManager);
    }

    private void OnClothingUnequipped(EntityUid uid, ClothingComponent clothingComponent, DidUnequipEvent args)
    {
        // Yes, this is using the trait system functions. I'm not going to write an entire third function library to do this.
        // They're generic for a reason.
        foreach (var function in clothingComponent.OnUnequipFunctions)
            function.OnPlayerSpawn(uid, _componentFactory, EntityManager, _serializationManager);
    }
}
