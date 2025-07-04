// SPDX-FileCopyrightText: 2022 Nemanja
// SPDX-FileCopyrightText: 2023 AJCM-git
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 themias
// SPDX-FileCopyrightText: 2024 Leon Friedrich
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2024 sleepyyapril <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>
// SPDX-FileCopyrightText: 2025 ash lea
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Rotation;
using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Rotation;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.GameStates;
using Robust.Shared.Timing;


namespace Content.Client.Buckle;

internal sealed class BuckleSystem : SharedBuckleSystem
{
    [Dependency] private readonly RotationVisualizerSystem _rotationVisualizerSystem = default!;
    [Dependency] private readonly IPlayerManager _player = default!; // Floof
    [Dependency] private readonly IGameTiming _timing = default!; // Floof
    [Dependency] private readonly SharedTransformSystem _xform = default!; // Floof

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StrapComponent, MoveEvent>(OnStrapMoveEvent);
        SubscribeLocalEvent<BuckleComponent, BuckledEvent>(OnBuckledEvent);
        SubscribeLocalEvent<BuckleComponent, UnbuckledEvent>(OnUnbuckledEvent);
    }

    // Floof section - update the draw depths of all buckled entities
    public override void FrameUpdate(float frameTime)
    {
        var query = EntityQueryEnumerator<StrapComponent>();
        while (query.MoveNext(out var uid, out var strap))
        {
            UpdateBucklesDrawDepth(uid, strap);
        }
        query.Dispose();
    }
    // Floof section end

    /// <summary>
    /// Is the strap entity already rotated north? Lower the draw depth of the buckled entity.
    /// </summary>
    private void OnBuckledEvent(Entity<BuckleComponent> ent, ref BuckledEvent args)
    {
        if (!TryComp<SpriteComponent>(args.Strap, out var strapSprite) ||
            !TryComp<SpriteComponent>(ent.Owner, out var buckledSprite))
            return;

        if (GetEntityOrientation(args.Strap.Owner) == Direction.North)  // Floof - replaced with a method call
        {
            ent.Comp.OriginalDrawDepth ??= buckledSprite.DrawDepth;
            buckledSprite.DrawDepth = strapSprite.DrawDepth - 1;
        }
    }

    /// <summary>
    /// Was the draw depth of the buckled entity lowered? Reset it upon unbuckling.
    /// </summary>
    private void OnUnbuckledEvent(Entity<BuckleComponent> ent, ref UnbuckledEvent args)
    {
        if (!TryComp<SpriteComponent>(ent.Owner, out var buckledSprite))
            return;

        if (ent.Comp.OriginalDrawDepth.HasValue)
        {
            buckledSprite.DrawDepth = ent.Comp.OriginalDrawDepth.Value;
            // Floof - do not reset original draw depth here because prediction FUCKING SUCKS
            // ent.Comp.OriginalDrawDepth = null;
        }
    }

    private void OnStrapMoveEvent(EntityUid uid, StrapComponent component, ref MoveEvent args)
    {
        // I'm moving this to the client-side system, but for the sake of posterity let's keep this comment:
        // > This is mega cursed. Please somebody save me from Mr Buckle's wild ride

        // The nice thing is its still true, this is quite cursed, though maybe not omega cursed anymore.
        // This code is garbage, it doesn't work with rotated viewports. I need to finally get around to reworking
        // sprite rendering for entity layers & direction dependent sorting.

        if (args.NewRotation == args.OldRotation)
            return;

        // Floof - everything that was below was separated into that method in order to allow calling it from other places
        UpdateBucklesDrawDepth(uid, component);
    }

    private void UpdateBucklesDrawDepth(EntityUid uid, StrapComponent component) {
        if (!TryComp<SpriteComponent>(uid, out var strapSprite))
            return;

        // Floof - man, fuck prediction.
        if (!_timing.IsFirstTimePredicted)
            return;

        var isNorth = GetEntityOrientation(uid) == Direction.North; // Floof - replaced with a method call
        foreach (var buckledEntity in component.BuckledEntities)
        {
            if (!TryComp<BuckleComponent>(buckledEntity, out var buckle))
                continue;

            if (!TryComp<SpriteComponent>(buckledEntity, out var buckledSprite))
                continue;

            if (isNorth)
            {
                buckle.OriginalDrawDepth ??= buckledSprite.DrawDepth;
                buckledSprite.DrawDepth = strapSprite.DrawDepth - 1;
            }
            else if (buckle.OriginalDrawDepth.HasValue)
            {
                buckledSprite.DrawDepth = buckle.OriginalDrawDepth.Value;
                buckle.OriginalDrawDepth = null;
            }
        }
    }

    // Floof section - method for getting the direction of an entity perceived by the local player
    private Direction GetEntityOrientation(EntityUid uid)
    {
        var xform = Transform(uid);
        var ownRotation = xform.LocalRotation;
        var eyeRotation =
            TryComp<EyeComponent>(_player.LocalEntity, out var eye) ? eye.Eye.Rotation : Angle.Zero;

        // This is TOTALLY dumb, but the eye stores camera rotation relative to the WORLD, so we need to convert it to local rotation as well
        // Cameras are also relative to grids (NOT direct parents), so we cannot just GetWorldRotation of the entity or something similar.
        if (xform.GridUid is { Valid: true } grid)
            eyeRotation += _xform.GetWorldRotation(grid);

        // Note: we subtract instead of adding because e.g. rotating an eye +90° visually rotates all entities in vision by -90°
        return (ownRotation + eyeRotation).GetCardinalDir();
    }
    // Floof section end
}
