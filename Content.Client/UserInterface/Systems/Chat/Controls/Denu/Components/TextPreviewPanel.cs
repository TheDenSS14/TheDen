using System;
using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu.Components;


public sealed class TextPreviewPanel : PanelContainer
{
    private readonly OutputPanel _outputPanel;
    private string? _lastText;
    private bool _autoExpandVertical;

    public bool ScrollFollowing { get; set; } = true;

    public bool AutoExpandVertical
    {
        get => _autoExpandVertical;
        set
        {
            if (_autoExpandVertical == value)
                return;

            _autoExpandVertical = value;
            InvalidateMeasure();
        }
    }

    public FormattedMessage? Message
    {
        get => null;
        set => SetMessage(value);
    }

    public string Text
    {
        get => _lastText ?? string.Empty;
        set => SetText(value);
    }

    public TextPreviewPanel()
    {
        _outputPanel = new OutputPanel { ScrollFollowing = false };
        AddChild(_outputPanel);
    }

    public void SetText(string text)
    {
        if (_lastText == text)
            return;

        _lastText = text;

        if (string.IsNullOrEmpty(text))
        {
            Clear();
            return;
        }

        _outputPanel.Clear();
        _outputPanel.AddText(text);

        if (ScrollFollowing && !AutoExpandVertical)
            _outputPanel.ScrollToBottom();
    }

    public void SetMarkup(string markup)
    {
        if (_lastText == markup)
            return;

        _lastText = markup;

        if (string.IsNullOrEmpty(markup))
        {
            Clear();
            return;
        }

        try
        {
            FormattedMessage message = FormattedMessage.FromMarkupOrThrow(markup);
            _outputPanel.Clear();
            _outputPanel.AddMessage(message);
        }
        catch (Exception)
        {
            _outputPanel.Clear();
            _outputPanel.AddText(markup);
        }

        if (ScrollFollowing && !AutoExpandVertical)
            _outputPanel.ScrollToBottom();
    }

    public void SetMessage(FormattedMessage? message)
    {
        _lastText = message?.ToMarkup();

        if (message == null)
        {
            Clear();
            return;
        }

        _outputPanel.Clear();
        _outputPanel.AddMessage(message);

        if (ScrollFollowing && !AutoExpandVertical)
            _outputPanel.ScrollToBottom();
    }

    public void Clear()
    {
        _lastText = null;
        _outputPanel.Clear();
    }

    public void ScrollToBottom()
    {
        _outputPanel.ScrollToBottom();
    }

    protected override Vector2 MeasureOverride(Vector2 availableSize)
    {
        if (AutoExpandVertical)
        {
            _outputPanel.Measure(new Vector2(availableSize.X, float.PositiveInfinity));
            Vector2 desiredSize = _outputPanel.DesiredSize;
            StyleBox? styleBox = GetStyleBox();
            Vector2 styleSize = styleBox?.MinimumSize ?? Vector2.Zero;
            return desiredSize + styleSize;
        }

        return base.MeasureOverride(availableSize);
    }
}
