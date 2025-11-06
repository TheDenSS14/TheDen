using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Hub;

[Serializable, NetSerializable]
public sealed record HubServer(
    string ServerId,
    string ConnectAddress,
    string? DisplayName,
    int? Players,
    int? MaxPlayers,
    bool IsOnline);

[Serializable, NetSerializable]
public sealed class HubServersUpdatedEvent(Dictionary<string, HubServer> servers) : EntityEventArgs
{
    public Dictionary<string, HubServer> Servers { get; init; } = servers;
}
