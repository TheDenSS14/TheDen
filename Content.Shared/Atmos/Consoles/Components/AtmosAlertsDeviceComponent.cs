// SPDX-FileCopyrightText: 2024 zelezniciar1 <39102800+zelezniciar1@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Components;

[RegisterComponent, NetworkedComponent]
[Access([])]
public sealed partial class AtmosAlertsDeviceComponent : Component
{
    /// <summary>
    /// The group that the entity belongs to
    /// </summary>
    [DataField, ViewVariables]
    public AtmosAlertsComputerGroup Group;
}
