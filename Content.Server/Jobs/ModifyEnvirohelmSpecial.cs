// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Inventory;
using Content.Shared.Roles;
using Content.Shared.Tag;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.Jobs;

[UsedImplicitly]
public sealed partial class ModifyEnvirohelmSpecial : JobSpecial
{
    // <summary>
    //   The new power cell of the envirohelm.
    // </summary>
    [DataField(required: true)]
    public EntProtoId PowerCell { get; private set; }

    [ValidatePrototypeId<SpeciesPrototype>]
    private const string Species = "Plasmaman";

    [ValidatePrototypeId<TagPrototype>]
    private const string Tag = "Envirohelm";

    private const string Slot = "head";

    private const string ItemSlot = "cell_slot";

    public override void AfterEquip(EntityUid mob)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        if (!entMan.TryGetComponent<HumanoidAppearanceComponent>(mob, out var appearanceComp) ||
            appearanceComp.Species != Species ||
            !entMan.System<InventorySystem>().TryGetSlotEntity(mob, Slot, out var helmet) ||
            helmet is not { } envirohelm ||
            !entMan.System<TagSystem>().HasTag(envirohelm, Tag) ||
            !entMan.TryGetComponent<ItemSlotsComponent>(envirohelm, out var itemSlotsComp))
            return;

        var itemSlotsSystem = entMan.System<ItemSlotsSystem>();

        if (itemSlotsSystem.GetItemOrNull(envirohelm, ItemSlot, itemSlotsComp) is { } powerCellToDelete)
            entMan.DeleteEntity(powerCellToDelete);

        var powerCell = entMan.Spawn(PowerCell);

        if (!itemSlotsSystem.TryInsert(envirohelm, ItemSlot, powerCell, null, itemSlotsComp, excludeUserAudio: true))
            entMan.DeleteEntity(powerCell);
    }
}
