// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.GameObjects;
using Content.Shared.Fax.Components;
using Content.Shared.Fax;
using Robust.Client.Animations;

namespace Content.Client.Fax.System;

/// <summary>
/// Visualizer for the fax machine which displays the correct sprite based on the inserted entity.
/// </summary>
public sealed class FaxVisualsSystem : EntitySystem
{
    [Dependency] private readonly AnimationPlayerSystem _player = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FaxMachineComponent, AppearanceChangeEvent>(OnAppearanceChanged);
    }

    private void OnAppearanceChanged(EntityUid uid, FaxMachineComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (_appearance.TryGetData(uid, FaxMachineVisuals.VisualState, out FaxMachineVisualState visuals) && visuals == FaxMachineVisualState.Inserting)
        {
            _player.Play(uid, new Animation()
            {
                Length = TimeSpan.FromSeconds(2.4),
                AnimationTracks =
                {
                    new AnimationTrackSpriteFlick()
                    {
                        LayerKey = FaxMachineVisuals.VisualState,
                        KeyFrames =
                        {
                            new AnimationTrackSpriteFlick.KeyFrame(component.InsertingState, 0f),
                            new AnimationTrackSpriteFlick.KeyFrame("icon", 2.4f),
                        }
                    }
                }
            }, "faxecute");
        }
    }
}
