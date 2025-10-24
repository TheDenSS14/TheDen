// SPDX-FileCopyrightText: 2022 Fishfish458
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 fishfish458
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2024 Plykiya
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Client.GameObjects;

using static Content.Shared.Paper.PaperComponent;

namespace Content.Client.Paper.UI;

public sealed class PaperVisualizerSystem : VisualizerSystem<PaperVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, PaperVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (AppearanceSystem.TryGetData<PaperStatus>(uid, PaperVisuals.Status , out var writingStatus, args.Component))
            args.Sprite.LayerSetVisible(PaperVisualLayers.Writing, writingStatus == PaperStatus.Written);

        if (AppearanceSystem.TryGetData<string>(uid, PaperVisuals.Stamp, out var stampState, args.Component))
        {
            args.Sprite.LayerSetState(PaperVisualLayers.Stamp, stampState);
            args.Sprite.LayerSetVisible(PaperVisualLayers.Stamp, true);
        }
    }
}

public enum PaperVisualLayers
{
    Stamp,
    Writing
}
