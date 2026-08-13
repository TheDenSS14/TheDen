using Content.Shared._DEN.Redial;
using Robust.Shared.Network;


namespace Content.Server._DEN.Redial;


/// <summary>
/// This handles redialing to a specific server.
/// </summary>
public sealed class RedialManager
{
    [Dependency] private readonly INetManager _netManager = null!;

    private SavedRedial? _savedRedial = null;

    public void Initialize() => _netManager.RegisterNetMessage<MsgRequestRedial>();

    public void SendSavedRedialPopup(INetChannel channel)
    {
        if (_savedRedial == null
            || string.IsNullOrEmpty(_savedRedial.RedialAddress)
            || string.IsNullOrEmpty(_savedRedial.RedialMessage))
            return;

        SendRedialPopup(channel, _savedRedial.RedialAddress, _savedRedial.RedialMessage);
    }

    public void SendRedialPopup(INetChannel channel, string redialAddress, string message)
    {
        var redialRequest = new MsgRequestRedial
        {
            RedialAddress = redialAddress,
            RedialMessage = message
        };

        _netManager.ServerSendMessage(redialRequest, channel);
    }

    public void SetSavedRedial(string redialAddress, string redialMessage) =>
        _savedRedial = new(redialAddress, redialMessage);

    public void ClearSavedRedial() => _savedRedial = null;

    public string GetRedialMessage() => _savedRedial == null ? string.Empty : _savedRedial.RedialMessage;
}

internal sealed class SavedRedial(string redialAddress, string redialMessage)
{
    public string RedialAddress = redialAddress;
    public string RedialMessage = redialMessage;
}
