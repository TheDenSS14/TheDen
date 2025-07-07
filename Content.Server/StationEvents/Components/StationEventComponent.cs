// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 LudwigVonChesterfield
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.StationEvents.Components;

/// <summary>
///     Defines basic data for a station event
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(Other = AccessPermissions.ReadExecute)]
public sealed partial class StationEventComponent : Component
{
    public const float WeightVeryLow = 0.0f;
    public const float WeightLow = 5.0f;
    public const float WeightNormal = 10.0f;
    public const float WeightHigh = 15.0f;
    public const float WeightVeryHigh = 20.0f;

    [DataField]
    public float Weight = WeightNormal;

    [DataField]
    public bool StartAnnouncement;

    [DataField]
    public bool EndAnnouncement;

    /// <summary>
    ///     In minutes, when is the first round time this event can start
    /// </summary>
    [DataField]
    public int EarliestStart = 5;

    /// <summary>
    ///     In minutes, the amount of time before the same event can occur again
    /// </summary>
    [DataField]
    public int ReoccurrenceDelay = 30;

    /// <summary>
    ///     How long after being added does the event start
    /// </summary>
    [DataField]
    public TimeSpan StartDelay = TimeSpan.Zero;

    /// <summary>
    ///     How long the event lasts.
    /// </summary>
    [DataField]
    public TimeSpan? Duration = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     The max amount of time the event lasts.
    /// </summary>
    [DataField]
    public TimeSpan? MaxDuration;

    /// <summary>
    ///     How many players need to be present on station for the event to run
    /// </summary>
    /// <remarks>
    ///     To avoid running deadly events with low-pop
    /// </remarks>
    [DataField]
    public int MinimumPlayers;

    /// <summary>
    ///     How many times this even can occur in a single round
    /// </summary>
    [DataField]
    public int? MaxOccurrences;

    /// <summary>
    /// When the station event starts.
    /// </summary>
    [DataField("startTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [Access(Other = AccessPermissions.ReadWriteExecute)]
    [AutoPausedField]
    public TimeSpan StartTime;

    /// <summary>
    /// When the station event ends.
    /// </summary>
    [DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [Access(Other = AccessPermissions.ReadWriteExecute)]
    [AutoPausedField]
    public TimeSpan? EndTime;
}
