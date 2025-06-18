// SPDX-FileCopyrightText: 2025 Cami <147159915+Camdot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chat.TypingIndicator;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class DenuUIController : UIController
{
    [UISystemDependency] private readonly TypingIndicatorSystem _typingIndicatorSystem = default!;

    public bool IsOpen { get; set; } = false;
    public bool AutoFormatterEnabled { get; set; } = false;
    public bool RemoveAsterisks { get; set; }

    public Color DialogueColor { get; set; } = Color.FromHex("#FFFFFF");
    public Color EmoteColor { get; set; } = Color.FromHex("#FF13FF");

    private DenuWindow? _denuWindow;

    public DenuUIController()
    {
    }

    public void CreateWindow()
    {
        if (!UIManager.TryGetFirstWindow<DenuWindow>(out _denuWindow))
            _denuWindow = UIManager.CreateWindow<DenuWindow>();
        
        _denuWindow!.OnOpen += () => IsOpen = true;
        _denuWindow!.OnClose += () => IsOpen = false;
    }

    public void OpenWindow()
    {
        if (_denuWindow is null)
            CreateWindow();
        _denuWindow!.OpenCentered();
    }

    public void CloseWindow()
    {
        _denuWindow!.Close();
    }

    public string FormatMessage(string message) =>
        MessageFormatter.Format(message, DialogueColor.ToHex(), EmoteColor.ToHex(), RemoveAsterisks);

    public void ShowTypingIndicator() =>
        _typingIndicatorSystem.ClientChangedChatText();

    public void HideTypingIndicator() =>
        _typingIndicatorSystem.ClientSubmittedChatText();


}
