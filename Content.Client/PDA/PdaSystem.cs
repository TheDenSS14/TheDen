// SPDX-FileCopyrightText: 2021 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.PDA;
using Content.Shared.Light;
using Robust.Client.GameObjects;

namespace Content.Client.PDA;

public sealed class PdaSystem : SharedPdaSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PdaComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(EntityUid uid, PdaComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (Appearance.TryGetData<bool>(uid, UnpoweredFlashlightVisuals.LightOn, out var isFlashlightOn, args.Component))
            args.Sprite.LayerSetVisible(PdaVisualLayers.Flashlight, isFlashlightOn);

        if (Appearance.TryGetData<bool>(uid, PdaVisuals.IdCardInserted, out var isCardInserted, args.Component))
            args.Sprite.LayerSetVisible(PdaVisualLayers.IdLight, isCardInserted);
    }

    protected override void OnComponentInit(EntityUid uid, PdaComponent component, ComponentInit args)
    {
        base.OnComponentInit(uid, component, args);

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (component.State != null)
            sprite.LayerSetState(PdaVisualLayers.Base, component.State);

        sprite.LayerSetVisible(PdaVisualLayers.Flashlight, component.FlashlightOn);
        sprite.LayerSetVisible(PdaVisualLayers.IdLight, component.IdSlot.StartingItem != null);
    }

    public enum PdaVisualLayers : byte
    {
        Base,
        Flashlight,
        IdLight
    }
}
