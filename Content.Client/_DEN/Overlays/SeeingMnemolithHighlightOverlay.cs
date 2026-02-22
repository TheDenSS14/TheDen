// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using System.Numerics;
using Content.Shared.Drugs;
using Content.Shared.Body.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Client.Drugs;

public sealed class MnemolithHighlightOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly TransformSystem _transform;
    private readonly VisibilitySystem _visibility;

    public override bool RequestScreenTexture => false;
    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly List<MnemolithRenderEntry> _entries = [];

    public SeeingMnemolithHighlightComponent? Comp;

    public MnemolithHighlightOverlay()
    {
        IoCManager.InjectDependencies(this);

        _transform = _entity.System<TransformSystem>();
        _visibility = _entity.System<VisibilitySystem>();

        ZIndex = -1;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (Comp is null)
            return;

        var worldHandle = args.WorldHandle;
        var eye = args.Viewport.Eye;

        if (eye == null)
            return;

        var player = _player.LocalEntity;
        if (!_entity.TryGetComponent(player, out TransformComponent? playerXform))
            return;

        var accumulator = Math.Clamp(Comp.PulseAccumulator, 0f, Comp.PulseTime);
        var alpha = Comp.PulseTime <= 0 ? 1f : float.Lerp(1f, 0f, accumulator / Comp.PulseTime);

        var mapId = eye.Position.MapId;
        var eyeRot = eye.Rotation;

        _entries.Clear();
        var entities = _entity.EntityQueryEnumerator<BodyComponent, SpriteComponent, TransformComponent>();
        while (entities.MoveNext(out var uid, out var body, out var sprite, out var xform))
        {
            if (!body.ThermalVisibility)
                continue;

            // TODO: If I understand robust right, this SHOULD check for LOS. Not super certain. Needs testing? Remove most of this comment after testing plsthx.
            if (!_visibility.InView(eye, uid))
                continue;

            if (_entries.Any(e => e.Ent.Owner == uid))
                continue;

            _entries.Add(new MnemolithRenderEntry((uid, sprite, xform), mapId, eyeRot));
        }

        foreach (var entry in _entries)
        {
            Render(entry.Ent, entry.Map, worldHandle, entry.EyeRot, Comp.Color, alpha);
        }

        worldHandle.SetTransform(Matrix3x2.Identity);
    }

    private void Render(Entity<SpriteComponent, TransformComponent> ent,
        MapId? map,
        DrawingHandleWorld handle,
        Angle eyeRot,
        Color color,
        float alpha)
    {
        var (uid, sprite, xform) = ent;
        if (xform.MapID != map)
            return;

        var position = _transform.GetWorldPosition(xform);
        var rotation = _transform.GetWorldRotation(xform);

        var originalColor = sprite.Color;
        sprite.Color = color.WithAlpha(alpha);
        sprite.Render(handle, eyeRot, rotation, position: position);
        sprite.Color = originalColor;
    }
}

public record struct MnemolithRenderEntry(
    Entity<SpriteComponent, TransformComponent> Ent,
    MapId? Map,
    Angle EyeRot);
