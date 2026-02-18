using Content.Client.Chat;
using Content.Client.UserInterface.Systems.Chat;
using Content.Client.UserInterface.Systems.Chat.Controls;
using Content.Client.UserInterface.Systems.Chat.Widgets;
using Content.Shared._DEN.Earmuffs;
using Content.Shared.Chat;
using Content.Shared.Chat.TypingIndicator;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;


namespace Content.Client._DEN.Earmuffs;


public sealed class EarmuffsSystem : SharedEarmuffsSystem
{
    [Dependency] private readonly ChatSystem _chat = null!;
    [Dependency] private readonly IOverlayManager _overlay = null!;
    [Dependency] private readonly IUserInterfaceManager _ui = null!;

    private CircleOverlay? _circleOverlay;
    private bool _useCircleOverlay;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeAllEvent<TypingChangedEvent>(OnTypingChanged);
    }

    private void OnTypingChanged(TypingChangedEvent ev)
    {
        if (!_useCircleOverlay)
            return;

        var range = GetRange();

        if (ev.State != TypingIndicatorState.Typing || range == null)
            return;

        if (_circleOverlay == null)
        {
            _circleOverlay = new();
            _circleOverlay.OnFullyFaded += RemoveCircleOverlay;
            _overlay.AddOverlay(_circleOverlay);
        }

        _circleOverlay.Range = range.Value;
        _circleOverlay.ShowCircle();
    }

    public void UpdateTypingUsesCircleOverlay(bool value)
    {
        _useCircleOverlay = value;
    }

    public void UpdateEarmuffs(float range)
    {
        var msg = new EarmuffsUpdated(range);
        RaiseNetworkEvent(msg);
    }

    private float? GetRange()
    {
        var chatBox = _ui.ActiveScreen?.GetWidget<ChatBox>() ?? _ui.ActiveScreen?.GetWidget<ResizableChatBox>();

        if (chatBox == null)
            return null;

        if (chatBox.SelectedChannel != ChatSelectChannel.Whisper
            && chatBox.SelectedChannel != ChatSelectChannel.Subtle
            && chatBox.SelectedChannel != ChatSelectChannel.SubtleOOC)
            return null;

        return chatBox.SelectedChannel is ChatSelectChannel.Whisper
            or ChatSelectChannel.Subtle
            or ChatSelectChannel.SubtleOOC
            ? _chat.WhisperClearRange
            : _chat.VoiceRange;
    }

    private void RemoveCircleOverlay()
    {
        if (_circleOverlay != null)
        {
            _overlay.RemoveOverlay(_circleOverlay);
            _circleOverlay = null;
        }
    }
}
