// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Gateway;

/// <summary>
/// Sent from client to server upon taking a gateway destination.
/// </summary>
[Serializable, NetSerializable]
public sealed class GatewayDestinationMessage : EntityEventArgs
{
    public int Index;
}
