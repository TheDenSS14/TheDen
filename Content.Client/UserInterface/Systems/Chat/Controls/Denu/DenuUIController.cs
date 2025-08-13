// SPDX-FileCopyrightText: 2025 Cam
// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Client._DEN.Earmuffs;
using Content.Client.Chat.TypingIndicator;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.Controllers;
using Range = Robust.Client.UserInterface.Controls.Range;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class DenuUIController : UIController
{
    [UISystemDependency] private readonly TypingIndicatorSystem _typingIndicatorSystem = default!;
    [UISystemDependency] private readonly EarmuffsSystem _earmuffsSystem = default!;
    [Dependency] private readonly IOverlayManager _overlayManager = default!;

    public bool AutoFormatterEnabled { get; set; } = false;

    public bool IsOpen { get; set; } = false;

    public MessageFormatter.FormatterConfig FormatterConfig { get; set; } = new MessageFormatter.FormatterConfig()
    {
        Rules = new()
        {
            new("***", "[bolditalic]", "[/bolditalic]", false, false),
            new("**", "[bold]", "[/bold]", false, false),
            new("\"", "[color={DialogueColor}]\"", "\"[/color]", false, true),
            new("*", "[italic]", "[/italic]", true, false),
            new("*", "[italic][color={EmoteColor}]*", "*[/color][/italic]", false, false),
        },
        Replacements = new()
        {
            { "DialogueColor", "#FFFFFF" },
            { "EmoteColor", "#FF13FF" }
        },
        AllowEscaping = true,
        EscapableTokens = new() { '*', '"', '\\' },
        RemoveAsterisks = false
    };

    private DenuWindow? _denuWindow;
    private CircleOverlay? _circleOverlay;

    public void CreateWindow()
    {
        if (!UIManager.TryGetFirstWindow(out _denuWindow))
            _denuWindow = UIManager.CreateWindow<DenuWindow>();

        _denuWindow!.OnOpen += () =>
        {
            _denuWindow.RecenterWindow(new(0.5f, 0.5f));
            IsOpen = true;
        };

        _denuWindow!.OnClose += () => IsOpen = false;
    }

    public void OpenWindow()
    {
        if (_denuWindow is not { Disposed: false })
            CreateWindow();

        _denuWindow!.OpenCentered();
    }

    public void CloseWindow()
    {
        _denuWindow!.Close();
    }

    public Color GetColorReplacement(string replacementName)
    {
        if (FormatterConfig.Replacements.TryGetValue(replacementName, out var colorHex))
            return Color.TryFromHex(colorHex) ?? Color.Magenta;
        return Color.Magenta;
    }

    public void SetColorReplacement(string replacementName, Color color) =>
        FormatterConfig.Replacements[replacementName] = color.ToHex();

    public string FormatMessage(string message, bool allowEscape = false) =>
        MessageFormatter.Format(message, FormatterConfig);

    public void ShowTypingIndicator() =>
        _typingIndicatorSystem.ClientChangedChatText();

    public void HideTypingIndicator() =>
        _typingIndicatorSystem.ClientSubmittedChatText();

    public void SetEarmuffRange(float range, bool sendUpdate)
    {
        if (_circleOverlay == null)
        {
            _circleOverlay = new();
            _circleOverlay.OnFullyFaded += RemoveCircleOverlay;
            _overlayManager.AddOverlay(_circleOverlay);
        }

        _circleOverlay.Range = range;
        _circleOverlay.ShowCircle();

        if (sendUpdate)
            _earmuffsSystem.UpdateEarmuffs(range);
    }

    private void RemoveCircleOverlay()
    {
        if (_circleOverlay != null)
        {
            _overlayManager.RemoveOverlay(_circleOverlay);
            _circleOverlay = null;
        }
    }
}
