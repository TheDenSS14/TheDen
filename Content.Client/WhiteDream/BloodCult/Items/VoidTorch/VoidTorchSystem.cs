// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 spess-empyrean <spessempyrean@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Light.Components;
using Content.Client.Light.EntitySystems;
using Content.Shared.WhiteDream.BloodCult;
using Content.Shared.WhiteDream.BloodCult.Items.VoidTorch;
using Robust.Client.GameObjects;

namespace Content.Client.WhiteDream.BloodCult.Items.VoidTorch;

public sealed class VoidTorchSystem : VisualizerSystem<VoidTorchComponent>
{
    [Dependency] private readonly LightBehaviorSystem _lightBehavior = default!;

    protected override void OnAppearanceChange(EntityUid uid,
        VoidTorchComponent component,
        ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;
        if (!AppearanceSystem.TryGetData<bool>(uid, GenericCultVisuals.State, out var state)
            || !TryComp<LightBehaviourComponent>(uid, out var lightBehaviour))
            return;

        _lightBehavior.StopLightBehaviour((uid, lightBehaviour));
        _lightBehavior.StartLightBehaviour((uid, lightBehaviour), state ? component.TurnOnLightBehaviour : component.TurnOffLightBehaviour);
    }
}
