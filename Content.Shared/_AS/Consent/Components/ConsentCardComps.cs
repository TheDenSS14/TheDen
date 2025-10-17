using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._AS.Consent.Components;

/// <summary>
/// Represents a consent card.
/// </summary>

[RegisterComponent, NetworkedComponent]
public sealed partial class ConsentCardComponent : Component
{
    /// <summary>
    /// The user who used this card.
    /// </summary>
    [DataField]
    public EntityUid User;

    /// <summary>
    /// Message sent to admins when card is used. Leave null to send no message.
    /// </summary>
    [DataField]
    public string? AdminMessage;

    /// <summary>
    /// Popup message sent to others when card is used.
    /// </summary>
    [DataField]
    public string PopupMessage = string.Empty;
}
