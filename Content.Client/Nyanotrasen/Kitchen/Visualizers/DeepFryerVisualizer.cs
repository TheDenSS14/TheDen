// SPDX-FileCopyrightText: 2023 JJ <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.GameObjects;
using Content.Client.Chemistry.Visualizers;
using Content.Client.Kitchen.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Kitchen.Components;
using Content.Shared.Nyanotrasen.Kitchen.Components;

namespace Content.Client.Kitchen.Visualizers
{
    public sealed class DeepFryerVisualizerSystem : VisualizerSystem<DeepFryerComponent>
    {
        [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;

        protected override void OnAppearanceChange(EntityUid uid, DeepFryerComponent component, ref AppearanceChangeEvent args)
        {
            if (!_appearanceSystem.TryGetData(uid, DeepFryerVisuals.Bubbling, out bool isBubbling) ||
                !TryComp<SolutionContainerVisualsComponent>(uid, out var scvComponent))
            {
                return;
            }

            scvComponent.FillBaseName = isBubbling ? "on-" : "off-";
        }
    }
}
