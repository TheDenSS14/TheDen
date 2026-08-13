using Content.Shared._DEN.Redial;
using Robust.Shared.Network;

namespace Content.Client._DEN.Redial;

public sealed class RedialManager
{
    [Dependency] private readonly INetManager _netManager = null!;

    private readonly HashSet<string> _declinedServers = new();

    public void Initialize() =>
        _netManager.RegisterNetMessage<MsgRequestRedial>(OnRedialRequest);

    private void OnRedialRequest(MsgRequestRedial msg)
    {
        var redialAddress = msg.RedialAddress;
        var redialMessage = msg.RedialMessage;

        if (_declinedServers.Contains(redialAddress))
            return;

        var redialPopup = new RedialPopup(redialAddress, redialMessage)
        {
            Resizable = false
        };

        redialPopup.OpenCentered();
        redialPopup.ButtonClicked += hide =>
        {
            if (!hide)
                return;

            _declinedServers.Add(redialMessage);
        };
    }
}
