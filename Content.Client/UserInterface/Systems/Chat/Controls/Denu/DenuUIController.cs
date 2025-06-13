using Content.Client.Chat.TypingIndicator;
using OpenToolkit.GraphicsLibraryFramework;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Timing;
using Serilog;

namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu;

public sealed partial class DenuUIController : UIController
{
    [UISystemDependency] private readonly TypingIndicatorSystem? _typingIndicator = default;
    
    public void TypingToggleUpdate()
    {
        _typingIndicator!.ClientChangedChatText();
    }

    public void TypingToggleDisabled()
    {
        _typingIndicator!.ClientSubmittedChatText();
    }

}