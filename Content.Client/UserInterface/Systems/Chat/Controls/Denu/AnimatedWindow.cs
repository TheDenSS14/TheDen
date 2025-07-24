using System.Numerics;
using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;

public abstract class AnimatedWindow : FancyWindow
{
    private const float AnimTime = 0.2f;

    private bool _animatingOpen = false;
    private bool _animatingClose = false;
    private float _animationTime = 0f;
    private Vector2 _targetSize;
    private Vector2 _targetPosition;
    private Vector2 _startSize;
    private Vector2 _startPosition;
    private Vector2 _originalMinSize;
    private Vector2 _originalMaxSize;
    private Vector2? _lastClosedPosition;
    private Vector2? _lastClosedSize;
    private float _startOpacity;
    private float _targetOpacity;

    protected override void Opened()
    {
        _startOpacity = 0f;
        _targetOpacity = 1f;
        
        Modulate = Color.White.WithAlpha(_startOpacity);
        
        base.Opened();
        
        _originalMinSize = MinSize;
        _originalMaxSize = MaxSize;

        Measure(Vector2Helpers.Infinity);
        Arrange(new UIBox2(Vector2.Zero, DesiredSize));
        
        Vector2 targetSize;
        if (_lastClosedSize.HasValue)
        {
            targetSize = _lastClosedSize.Value;
        }
        else if (DesiredSize.X > 0 && DesiredSize.Y > 0)
        {
            targetSize = DesiredSize;
        }
        else
        {
            targetSize = Size;
        }
        
        _targetSize = targetSize;
        if (_lastClosedPosition.HasValue)
        {
            _targetPosition = _lastClosedPosition.Value;
        }
        else
        {
            _targetPosition = Position;
        }

        var centerOffset = _targetSize / 2f;
        _startSize = Vector2.Zero;
        _startPosition = _targetPosition + centerOffset;

        MinSize = Vector2.Zero;
        MaxSize = new Vector2(10000f, 10000f);

        _animatingOpen = true;
        _animatingClose = false;
        _animationTime = 0f;

        SetSize = _startSize;
        LayoutContainer.SetPosition(this, _startPosition);
    }

    public override void Close()
    {
        if (_animatingClose)
            return;

        _lastClosedPosition = Position;
        _lastClosedSize = Size;

        _startSize = Size;
        _startPosition = Position;

        if (!_animatingOpen)
        {
            _originalMinSize = MinSize;
            _originalMaxSize = MaxSize;
        }

        MinSize = Vector2.Zero;
        MaxSize = new Vector2(10000f, 10000f);

        var center = _startPosition + _startSize / 2f;
        
        _targetSize = Vector2.Zero;
        _targetPosition = center;

        _startOpacity = 1f;
        _targetOpacity = 0f;

        _animatingClose = true;
        _animatingOpen = false;
        _animationTime = 0f;
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);
        
        if (_animatingOpen || _animatingClose)
        {
            _animationTime += args.DeltaSeconds;
            var progress = Math.Min(_animationTime / AnimTime, 1f);
            
            var easedProgress = EaseCubic(progress);
            
            var currentSize = Vector2.Lerp(_startSize, _targetSize, easedProgress);
            var currentPosition = Vector2.Lerp(_startPosition, _targetPosition, easedProgress);
            var currentOpacity = MathHelper.Lerp(_startOpacity, _targetOpacity, easedProgress);
            
            SetSize = currentSize;
            LayoutContainer.SetPosition(this, currentPosition);
            Modulate = Color.White.WithAlpha(currentOpacity);
            
            if (progress >= 1f)
            {
                if (_animatingOpen)
                {
                    _animatingOpen = false;
                    UserInterfaceManager.DeferAction(() =>
                    {
                        SetSize = _targetSize;
                        LayoutContainer.SetPosition(this, _targetPosition);
                        MinSize = _originalMinSize;
                        MaxSize = _originalMaxSize;
                    });
                    Modulate = Color.White.WithAlpha(1f);
                }
                else if (_animatingClose)
                {
                    _animatingClose = false;
                    UserInterfaceManager.DeferAction(() => 
                    {
                        MinSize = _originalMinSize;
                        MaxSize = _originalMaxSize;
                        base.Close();
                    });
                }
            }
        }
    }

    private static float EaseCubic(float t)
    {
        return CubicBezier(t, 0.25f, 0.1f, 0.25f, 1.0f);
    }
    
    private static float CubicBezier(float t, float p1x, float p1y, float p2x, float p2y)
    {
        var t2 = t * t;
        var t3 = t2 * t;
        var oneMinusT = 1f - t;
        var oneMinusT2 = oneMinusT * oneMinusT;
        var oneMinusT3 = oneMinusT2 * oneMinusT;
        
        return oneMinusT3 * 0f + 3f * oneMinusT2 * t * p1y + 3f * oneMinusT * t2 * p2y + t3 * 1f;
    }
}
