using Content.Shared._DEN.CCVars;
using Content.Shared._DEN.Hub;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Server._DEN.Hub;


/// <summary>
/// This handles refreshing the server hub.
/// </summary>
public sealed class HubRefreshSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = null!;
    [Dependency] private readonly IGameTiming _timing = null!;
    [Dependency] private readonly HubManager _hubManager = null!;

    private TimeSpan _nextRefresh = TimeSpan.Zero;
    private int _refreshInterval;

    public override void Initialize()
    {
        base.Initialize();

        _refreshInterval = _cfg.GetCVar(DenCCVars.LobbyRefreshServersInterval);
        Subs.CVar(_cfg, DenCCVars.LobbyRefreshServersInterval, OnRefreshIntervalUpdated);

        _hubManager.OnServersRefreshed += OnServersRefreshed;
    }

    private void OnServersRefreshed()
    {
        var refreshedEvent = new HubServersUpdatedEvent(_hubManager.Servers);
        RaiseNetworkEvent(refreshedEvent);
    }

    public override void Update(float frameTime)
    {
        if (_nextRefresh > _timing.RealTime)
            return;

        _hubManager.Refresh();
        _nextRefresh = _timing.RealTime + TimeSpan.FromSeconds(_refreshInterval);
    }

    private void OnRefreshIntervalUpdated(int newValue) => _refreshInterval = newValue;
}
