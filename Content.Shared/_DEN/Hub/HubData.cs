// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Hub;

[Serializable, NetSerializable]
public sealed record HubServer(
    string ServerId,
    string StatusUrl,
    string ConnectAddress,
    string? DisplayName,
    int? Players,
    int? MaxPlayers,
    bool IsOnline,
    bool CanConnect);

[Serializable, NetSerializable]
public sealed class HubServersUpdatedEvent(Dictionary<string, HubServer> servers) : EntityEventArgs
{
    public Dictionary<string, HubServer> Servers { get; init; } = servers;
}
