using System.Numerics;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Maths;
using Robust.Shared.Timing;


namespace Content.Client.UserInterface.Systems.Chat.Controls;


public sealed class CircleOverlay : Overlay
{
    [Dependency] private readonly IGameTiming _timing = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private const float CircleThickness = 0.05f;
    private const float OutlineThickness = 0.05f;
    private const float EaseInDuration = 0.5f;
    private const float EaseOutDuration = 2.5f;
    private const float FadeTime = EaseInDuration + EaseOutDuration; 
    private const int CircleSegments = 128; 
    private const float MaxOpacity = 1.0f;

    public float Range { get; set; } = 10.0f;

    private TimeSpan _showStartTime = TimeSpan.Zero;
    private bool _isVisible = false;

    public event Action? OnFullyFaded;

    public CircleOverlay()
    {
        IoCManager.InjectDependencies(this);
    }


    public void ShowCircle()
    {
        if (!_isVisible)
        {
            _showStartTime = _timing.CurTime;
            _isVisible = true;
        }
        else
        {
            _showStartTime = _timing.CurTime - TimeSpan.FromSeconds(EaseInDuration + 0.001f);
        }
    }

    private void DrawThickCircle(DrawingHandleBase handle, Vector2 center, float radius, float thickness, Color color)
    {
        var outerRadius = radius + thickness / 2f;
        var innerRadius = radius - thickness / 2f;

        if (innerRadius <= 0f)
        {
            var filledVertices = new List<Vector2>();

            filledVertices.Add(center);

            for (int i = 0; i <= CircleSegments; i++)
            {
                var angle = (float)(i * 2 * Math.PI / CircleSegments);
                var x = center.X + outerRadius * MathF.Cos(angle);
                var y = center.Y + outerRadius * MathF.Sin(angle);
                filledVertices.Add(new Vector2(x, y));
            }

            handle.DrawPrimitives(DrawPrimitiveTopology.TriangleFan, filledVertices, color);
            return;
        }

        var ringVertices = new List<Vector2>();

        for (int i = 0; i <= CircleSegments; i++)
        {
            var angle = (float)(i * 2 * Math.PI / CircleSegments);
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);

            var innerX = center.X + innerRadius * cos;
            var innerY = center.Y + innerRadius * sin;
            ringVertices.Add(new Vector2(innerX, innerY));

            var outerX = center.X + outerRadius * cos;
            var outerY = center.Y + outerRadius * sin;
            ringVertices.Add(new Vector2(outerX, outerY));
        }

        handle.DrawPrimitives(DrawPrimitiveTopology.TriangleStrip, ringVertices, color);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (!_isVisible)
            return;

        var elapsed = _timing.CurTime - _showStartTime;
        var elapsedSeconds = (float)elapsed.TotalSeconds;

        if (elapsedSeconds >= FadeTime)
        {
            _isVisible = false;
            OnFullyFaded?.Invoke(); 
            return;
        }

        var drawingHandle = args.DrawingHandle;

        if (args.Viewport.Eye?.Position == null)
            return;

        var playerWorldPos = args.Viewport.Eye.Position.Position;

        float easedAlpha;

        if (elapsedSeconds <= EaseInDuration)
        {
            var t = elapsedSeconds / EaseInDuration;
            easedAlpha = MathUtils.CubicBezier(0.42f, 0f, 1f, 1f, t);
        }
        else
        {
            var t = (elapsedSeconds - EaseInDuration) / EaseOutDuration;
            easedAlpha = MathUtils.CubicBezier(0f, 0f, 0.58f, 1f, 1.0f - t);
        }

        easedAlpha = Math.Max(0f, Math.Min(MaxOpacity, easedAlpha));

        var blackColor = Color.Black.WithAlpha(easedAlpha);
        DrawThickCircle(drawingHandle, playerWorldPos, Range, CircleThickness + 2 * OutlineThickness, blackColor);

        var whiteColor = Color.White.WithAlpha(easedAlpha);
        DrawThickCircle(drawingHandle, playerWorldPos, Range, CircleThickness, whiteColor);
    }
}
