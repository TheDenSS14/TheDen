// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Clock;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedClockSystem))]
[AutoGenerateComponentState]
public sealed partial class ClockComponent : Component
{
    /// <summary>
    /// If not null, this time will be permanently shown.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan? StuckTime;

    /// <summary>
    /// The format in which time is displayed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ClockType ClockType = ClockType.TwelveHour;

    [DataField]
    public string HoursBase = "hours_";

    [DataField]
    public string MinutesBase = "minutes_";
}

[Serializable, NetSerializable]
public enum ClockType : byte
{
    TwelveHour,
    TwentyFourHour
}

[Serializable, NetSerializable]
public enum ClockVisualLayers : byte
{
    HourHand,
    MinuteHand
}
