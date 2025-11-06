using Content.Shared._DEN.Hub;


namespace Content.Client._DEN.Hub;


public sealed class HubSystem : EntitySystem
{
    public event Action<HubServersUpdatedEvent>? OnHubServersUpdated;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<HubServersUpdatedEvent>(OnServersUpdated);
    }

    private void OnServersUpdated(HubServersUpdatedEvent ev)
    {
        OnHubServersUpdated?.Invoke(ev);
    }
}
