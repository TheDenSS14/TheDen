// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 RedFoxIV <38788538+RedFoxIV@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Effects;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Client.Effects;

public sealed class ColorFlashEffectSystem : SharedColorFlashEffectSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly AnimationPlayerSystem _animation = default!;
    [Dependency] private readonly IComponentFactory _factory = default!;

    /// <summary>
    /// It's a little on the long side but given we use multiple colours denoting what happened it makes it easier to register.
    /// </summary>
    private const float AnimationLength = 0.30f;
    private const string AnimationKey = "color-flash-effect";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeAllEvent<ColorFlashEffectEvent>(OnColorFlashEffect);
        SubscribeLocalEvent<ColorFlashEffectComponent, AnimationCompletedEvent>(OnEffectAnimationCompleted);
    }

    public override void RaiseEffect(Color color, List<EntityUid> entities, Filter filter, float? animationLength = null)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        OnColorFlashEffect(new ColorFlashEffectEvent(color, GetNetEntityList(entities), animationLength));
    }

    private void OnEffectAnimationCompleted(EntityUid uid, ColorFlashEffectComponent component, AnimationCompletedEvent args)
    {
        if (args.Key != AnimationKey)
            return;

        if (TryComp<SpriteComponent>(uid, out var sprite))
        {
            sprite.Color = component.Color;
        }

        // Floof - commented this out due to a race condition. It's quite complicated to explain;
        // in short terms, don't do deferred component removal when dealing with concurrent tasks concerning the same component.
        // RemCompDeferred<ColorFlashEffectComponent>(uid);
    }

    private Animation? GetDamageAnimation(EntityUid uid, Color color, SpriteComponent? sprite = null, float? animationLength = null)
    {
        if (!Resolve(uid, ref sprite, false))
            return null;

        // 90% of them are going to be this so why allocate a new class.
        return new Animation
        {
            Length = TimeSpan.FromSeconds(animationLength ?? AnimationLength),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Color),
                    InterpolationMode = AnimationInterpolationMode.Linear,
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(color, 0f),
                        new AnimationTrackProperty.KeyFrame(sprite.Color, animationLength ?? AnimationLength)
                    }
                }
            }
        };
    }

    private void OnColorFlashEffect(ColorFlashEffectEvent ev)
    {
        var color = ev.Color;

        foreach (var nent in ev.Entities)
        {
            var ent = GetEntity(nent);

            if (Deleted(ent))
            {
                continue;
            }

            var player = EnsureComp<AnimationPlayerComponent>(ent);

            // Need to stop the existing animation first to ensure the sprite color is fixed.
            // Otherwise we might lerp to a red colour instead.
            if (_animation.HasRunningAnimation(ent, player, AnimationKey))
            {
                _animation.Stop(ent, player, AnimationKey);
            }

            if (!TryComp<SpriteComponent>(ent, out var sprite))
            {
                continue;
            }

            // Floof - commented out. This is handled by the animation complete event.
            // if (TryComp<ColorFlashEffectComponent>(ent, out var effect))
            // {
            //     sprite.Color = effect.Color;
            // }


            var animation = GetDamageAnimation(ent, color, sprite, ev.AnimationLength);

            if (animation == null)  
                continue;

            var comp = EnsureComp<ColorFlashEffectComponent>(ent);

            comp.Color = sprite.Color;
            _animation.Play((ent, player), animation, AnimationKey);
        }
    }
}
