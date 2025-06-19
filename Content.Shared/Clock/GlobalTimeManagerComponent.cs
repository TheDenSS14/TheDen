// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Clock;

/// <summary>
/// This is used for globally managing the time on-station
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause, Access(typeof(SharedClockSystem))]
public sealed partial class GlobalTimeManagerComponent : Component
{
    /// <summary>
    /// A fixed random offset, used to fuzz the time between shifts.
    /// </summary>
    [DataField, AutoPausedField, AutoNetworkedField]
    public TimeSpan TimeOffset;
}
