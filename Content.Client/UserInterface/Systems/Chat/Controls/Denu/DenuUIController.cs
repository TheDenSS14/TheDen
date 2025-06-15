using Content.Client.Chat.TypingIndicator;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;


public sealed class DenuUIController : UIController
{
    [UISystemDependency] private readonly TypingIndicatorSystem _typingIndicatorSystem = default!;
    public bool AutoFormatterEnabled { get; set; } = false;
    public Color DialogueColor { get; set; } = Color.FromHex("#FFFFFF");
    public Color EmoteColor { get; set; } = Color.FromHex("#FF13FF");

    public string FormatMessage(string message) =>
        MessageFormatter.Format(message, DialogueColor.ToHex(), EmoteColor.ToHex());

    public void ShowTypingIndicator() =>
        _typingIndicatorSystem.ClientChangedChatText();

    public void HideTypingIndicator() =>
        _typingIndicatorSystem.ClientSubmittedChatText();
}
