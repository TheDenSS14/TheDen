// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Ensnaring;
using Content.Shared.Ensnaring.Components;
using Robust.Client.GameObjects;

namespace Content.Client.Ensnaring.Visualizers;

public sealed class EnsnareableSystem : SharedEnsnareableSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EnsnareableComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<EnsnareableComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnComponentInit(EntityUid uid, EnsnareableComponent component, ComponentInit args)
    {
        if(!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        // TODO remove this, this should just be in yaml.
        sprite.LayerMapReserveBlank(EnsnaredVisualLayers.Ensnared);
    }

    private void OnAppearanceChange(EntityUid uid, EnsnareableComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null || !args.Sprite.LayerMapTryGet(EnsnaredVisualLayers.Ensnared, out var layer))
            return;

        if (_appearance.TryGetData<bool>(uid, EnsnareableVisuals.IsEnsnared, out var isEnsnared, args.Component))
        {
            if (component.Sprite != null)
            {
                args.Sprite.LayerSetRSI(layer, component.Sprite);
                args.Sprite.LayerSetState(layer, component.State);
                args.Sprite.LayerSetVisible(layer, isEnsnared);
            }
        }
    }
}

public enum EnsnaredVisualLayers : byte
{
    Ensnared,
}
