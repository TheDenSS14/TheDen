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
    public bool AutoFormatterEnabled { get; set; }
    public bool RemoveAsterisks { get; set; }
    public Color DialogueColor { get; set; } = Color.FromHex("#FFFFFF");
    public Color EmoteColor { get; set; } = Color.FromHex("#FF13FF");

    public string FormatMessage(string message) =>
        MessageFormatter.Format(message, DialogueColor.ToHex(), EmoteColor.ToHex(), RemoveAsterisks);

    public void ShowTypingIndicator() =>
        _typingIndicatorSystem.ClientChangedChatText();

    public void HideTypingIndicator() =>
        _typingIndicatorSystem.ClientSubmittedChatText();
}
