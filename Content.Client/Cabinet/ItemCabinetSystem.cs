// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Cabinet;
using Robust.Client.GameObjects;

namespace Content.Client.Cabinet;

public sealed class ItemCabinetSystem : SharedItemCabinetSystem
{
    protected override void UpdateAppearance(EntityUid uid, ItemCabinetComponent? cabinet = null)
    {
        if (!Resolve(uid, ref cabinet))
            return;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        var state = cabinet.Opened ? cabinet.OpenState : cabinet.ClosedState;
        if (state != null)
            sprite.LayerSetState(ItemCabinetVisualLayers.Door, state);
        sprite.LayerSetVisible(ItemCabinetVisualLayers.ContainsItem, cabinet.CabinetSlot.HasItem);
    }
}

public enum ItemCabinetVisualLayers
{
    Door,
    ContainsItem
}
