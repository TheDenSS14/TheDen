using Content.Shared.FloofStation.Info;
using Robust.Shared.Network;


namespace Content.Client._Floof.Info;


public sealed class NsfwDisclaimerSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;

    private NsfwDisclaimerWindow? _window;

    public override void Initialize()
    {
        SubscribeNetworkEvent<ShowNsfwPopupDisclaimerMessage>(OnShowPopup);
        _net.Disconnect += (_, _) => _window?.Close();
    }

    private void OnShowPopup(ShowNsfwPopupDisclaimerMessage message)
    {
        _window?.Close();

        _window = new();
        _window.OpenCentered();
        _window.OnClose += () => _window = null;
    }
}
