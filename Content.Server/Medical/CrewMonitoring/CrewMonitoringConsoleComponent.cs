// SPDX-FileCopyrightText: 2021 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Medical.SuitSensor;
using Robust.Shared.Audio; // DeltaV
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom; // DeltaV

namespace Content.Server.Medical.CrewMonitoring;

[RegisterComponent, AutoGenerateComponentPause] // DeltaV - add AutoGenerateComponentPause
[Access(typeof(CrewMonitoringConsoleSystem))]
public sealed partial class CrewMonitoringConsoleComponent : Component
{
    /// <summary>
    ///     List of all currently connected sensors to this console.
    /// </summary>
    public Dictionary<string, SuitSensorStatus> ConnectedSensors = new();

    /// <summary>
    ///     After what time sensor consider to be lost.
    /// </summary>
    [DataField("sensorTimeout"), ViewVariables(VVAccess.ReadWrite)]
    public float SensorTimeout = 10f;

    // DeltaV - start of alert system code
    /// <summary>
    ///     Should the component beep if someone goes critical or dies
    /// </summary>
    [DataField]
    public bool AlertsEnabled = true;

    /// <summary>
    ///     Track sensors that have triggered the crew member critical alert.
    /// </summary>
    public HashSet<string> AlertedSensors = [];

    /// <summary>
    ///     Timestamp of the next possible alert (alert cooldown)
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextAlert;

    /// <summary>
    ///     Time between alerts
    /// </summary>
    [DataField]
    public TimeSpan AlertCooldown = TimeSpan.FromSeconds(15);

    /// <summary>
    ///     Alert sound that is played when a crew member goes into critical / dies.
    /// </summary>
    [DataField]
    public SoundSpecifier AlertSound = new SoundPathSpecifier("/Audio/_DV/Medical/CrewMonitoring/crew_alert.ogg");
    // DeltaV - end of alert system code
}
