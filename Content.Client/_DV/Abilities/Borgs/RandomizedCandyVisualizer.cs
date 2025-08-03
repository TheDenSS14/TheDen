// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Abilities.Borgs;
using Robust.Client.GameObjects;

namespace Content.Client._DV.Abilities.Borgs;

/// <summary>
/// Responsible for coloring randomized candy.
/// </summary>
public sealed class RandomizedCandyVisualizer : VisualizerSystem<RandomizedCandyComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, RandomizedCandyComponent component, ref AppearanceChangeEvent args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite)
            || !AppearanceSystem.TryGetData<Color>(uid, RandomizedCandyVisuals.Color, out var color, args.Component))
        {
            return;
        }

        sprite.LayerSetColor(CandyVisualLayers.Ball, color);
    }
}

public enum CandyVisualLayers : byte
{
    Ball
}
