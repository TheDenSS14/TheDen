using System.Linq;
using System.Numerics;
using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public abstract class AnimatedWindow : FancyWindow
{
    private const float AnimationTime = 0.05f;

    private bool _animatingOpen;
    private bool _animatingClose;
    private float _currentAnimationTime;
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
        InitializeOpenAnimation();
        base.Opened();
        SetupAnimationConstraints();
        StartOpenAnimation();
    }

    public override void Close()
    {
        if (_animatingClose)
            return;

        StartCloseAnimation();
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);
        UpdateAnimation(args);

        if (!_animatingOpen && !_animatingClose && Size.X < 50 && Size.Y < 50)
        {
            RestoreToValidSize();
        }
    }

    private void RestoreToValidSize()
    {
        Vector2 targetSize = _lastClosedSize ?? new Vector2(400, 600);
        Vector2 targetPosition = _lastClosedPosition ?? Position;

        SetSize = targetSize;
        LayoutContainer.SetPosition(this, targetPosition);
        RestoreOriginalConstraints();
    }

    private void InitializeOpenAnimation()
    {
        _startOpacity = 0f;
        _targetOpacity = 1f;
        Modulate = Color.White.WithAlpha(_startOpacity);
    }

    private void SetupAnimationConstraints()
    {
        if (_originalMinSize == Vector2.Zero && _originalMaxSize == Vector2.Zero)
        {
            _originalMinSize = MinSize;
            _originalMaxSize = MaxSize;
        }

        MinSize = Vector2.Zero;
        MaxSize = new Vector2(10000f, 10000f);
    }

    private void StartOpenAnimation()
    {
        Vector2 targetSize = DetermineTargetSize();
        _targetSize = targetSize;
        _targetPosition = DetermineTargetPosition();

        Vector2 centerOffset = _targetSize / 2f;
        _startSize = Vector2.Zero;
        _startPosition = _targetPosition + centerOffset;

        SetAnimationConstraints();
        StartAnimation(true);
    }

    private void StartCloseAnimation()
    {
        _lastClosedPosition = Position;
        _lastClosedSize = Size;

        _startSize = Size;
        _startPosition = Position;

        if (!_animatingOpen)
        {
            _originalMinSize = MinSize;
            _originalMaxSize = MaxSize;
        }

        Vector2 center = _startPosition + _startSize / 2f;
        _targetSize = Vector2.Zero;
        _targetPosition = center;

        _startOpacity = 1f;
        _targetOpacity = 0f;

        SetAnimationConstraints();
        StartAnimation(false);
    }

    private Vector2 DetermineTargetSize()
    {
        if (_lastClosedSize.HasValue)
        {
            return _lastClosedSize.Value;
        }

        if (DesiredSize.X > 0 && DesiredSize.Y > 0)
        {
            return DesiredSize;
        }

        return Size;
    }

    private Vector2 DetermineTargetPosition()
    {
        return _lastClosedPosition ?? Position;
    }

    private void SetAnimationConstraints()
    {
        if (_originalMinSize == Vector2.Zero && _originalMaxSize == Vector2.Zero)
        {
            _originalMinSize = MinSize;
            _originalMaxSize = MaxSize;
        }

        MinSize = Vector2.Zero;
        MaxSize = new Vector2(10000f, 10000f);
    }

    private void StartAnimation(bool isOpening)
    {
        _animatingOpen = isOpening;
        _animatingClose = !isOpening;
        _currentAnimationTime = 0f;

        if (isOpening)
        {
            SetSize = _startSize;
            LayoutContainer.SetPosition(this, _startPosition);
        }
    }

    private void UpdateAnimation(FrameEventArgs args)
    {
        if (!_animatingOpen && !_animatingClose)
            return;

        _currentAnimationTime += args.DeltaSeconds;
        float progress = Math.Min(_currentAnimationTime / AnimationTime, 1f);
        float easedProgress = MathUtils.EaseCubic(progress);

        ApplyAnimationFrame(easedProgress);

        if (progress >= 1f)
        {
            CompleteAnimation();
        }
    }

    private void ApplyAnimationFrame(float easedProgress)
    {
        Vector2 currentSize = Vector2.Lerp(_startSize, _targetSize, easedProgress);
        Vector2 currentPosition = Vector2.Lerp(_startPosition, _targetPosition, easedProgress);
        float currentOpacity = MathHelper.Lerp(_startOpacity, _targetOpacity, easedProgress);

        SetSize = currentSize;
        LayoutContainer.SetPosition(this, currentPosition);
        Modulate = Color.White.WithAlpha(currentOpacity);
    }

    private void CompleteAnimation()
    {
        if (_animatingOpen)
        {
            CompleteOpenAnimation();
        }
        else if (_animatingClose)
        {
            CompleteCloseAnimation();
        }
    }

    private void CompleteOpenAnimation()
    {
        _animatingOpen = false;
        UserInterfaceManager.DeferAction(() =>
        {
            SetSize = _targetSize;
            LayoutContainer.SetPosition(this, _targetPosition);
            RestoreOriginalConstraints();
        });
        Modulate = Color.White.WithAlpha(1f);
    }

    private void CompleteCloseAnimation()
    {
        _animatingClose = false;
        UserInterfaceManager.DeferAction(() =>
        {
            RestoreOriginalConstraints();
            base.Close();
        });
    }

    private void RestoreOriginalConstraints()
    {
        if (_originalMinSize != Vector2.Zero || _originalMaxSize != Vector2.Zero)
        {
            MinSize = _originalMinSize;
            MaxSize = _originalMaxSize;
        }
    }

    protected override void Resized()
    {
        base.Resized();

        if (!_animatingOpen && !_animatingClose && Size.X > 0 && Size.Y > 0)
        {
            _lastClosedSize = Size;
            _lastClosedPosition = Position;
        }
    }

    protected void RecurseChildren(Control.OrderedChildCollection children, Func<Control, bool> predicate, Action<Control> action, bool continueOnPredicateFail = true)
    {
        foreach (Control child in children)
        {
            RecurseChildren(child, predicate, action, continueOnPredicateFail);
        }
    }

    protected void RecurseChildren(Control root, Func<Control, bool> predicate, Action<Control> action, bool continueOnPredicateFail = true)
    {
        if (predicate(root))
        {
            action(root);
        }

        if (predicate(root) || continueOnPredicateFail)
        {
            foreach (Control child in root.Children)
            {
                RecurseChildren(child, predicate, action, continueOnPredicateFail);
            }
        }
    }

    protected Func<Control, bool> CreateTypePredicate(params Type[] types)
    {
        return control => types.Any(type => type.IsAssignableFrom(control.GetType()));
    }
}
