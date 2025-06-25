// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.Physics;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Dynamics.Joints;

namespace Content.Client.Physics;

/// <summary>
/// Draws a texture on top of a joint.
/// </summary>
public sealed class JointVisualsOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowFOV;

    private IEntityManager _entManager;

    private HashSet<Joint> _drawn = new();

    public JointVisualsOverlay(IEntityManager entManager)
    {
        _entManager = entManager;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        _drawn.Clear();
        var worldHandle = args.WorldHandle;
        // Floofstation: fix incorrect drawing box location due to incorrect coordinate system
        worldHandle.SetTransform(Vector2.Zero, Angle.Zero);

        var spriteSystem = _entManager.System<SpriteSystem>();
        var xformSystem = _entManager.System<SharedTransformSystem>();
        var joints = _entManager.EntityQueryEnumerator<JointVisualsComponent, TransformComponent>();
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        while (joints.MoveNext(out var visuals, out var xform))
        {
            if (xform.MapID != args.MapId)
                continue;

            var other = visuals.Target;

            if (!xformQuery.TryGetComponent(other, out var otherXform))
                continue;

            if (xform.MapID != otherXform.MapID)
                continue;

            var texture = spriteSystem.Frame0(visuals.Sprite);
            var width = texture.Width / (float) EyeManager.PixelsPerMeter;

            var coordsA = xform.Coordinates;
            var coordsB = otherXform.Coordinates;

            var rotA = xform.LocalRotation;
            var rotB = otherXform.LocalRotation;

            coordsA = coordsA.Offset(rotA.RotateVec(visuals.OffsetA));
            coordsB = coordsB.Offset(rotB.RotateVec(visuals.OffsetB));

            var posA = coordsA.ToMapPos(_entManager, xformSystem);
            var posB = coordsB.ToMapPos(_entManager, xformSystem);
            var diff = (posB - posA);
            var length = diff.Length();

            var midPoint = diff / 2f + posA;
            var angle = (posB - posA).ToWorldAngle();
            var box = new Box2(-width / 2f, -length / 2f, width / 2f, length / 2f);
            var rotate = new Box2Rotated(box.Translated(midPoint), angle, midPoint);

            worldHandle.DrawTextureRect(texture, rotate);
        }
    }
}
