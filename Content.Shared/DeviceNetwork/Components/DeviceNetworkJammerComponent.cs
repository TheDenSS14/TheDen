// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.DeviceNetwork.Components;

/// <summary>
/// Allow entities to jam DeviceNetwork packets.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DeviceNetworkJammerComponent : Component
{
    /// <summary>
    /// Range where packets will be jammed. This is checked both against the sender and receiver.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Range = 5.0f;

    /// <summary>
    /// Device networks that can be jammed. For a list of default NetworkIds see DeviceNetIdDefaults on Content.Server.
    /// Network ids are not guaranteed to be limited to DeviceNetIdDefaults.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<string> JammableNetworks = [];

}
