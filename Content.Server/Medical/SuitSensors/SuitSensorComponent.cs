// SPDX-FileCopyrightText: 2021 Alex Evgrashin
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Julian Giebel
// SPDX-FileCopyrightText: 2023 chromiumboy
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 SimpleStation14
// SPDX-FileCopyrightText: 2024 themias
// SPDX-FileCopyrightText: 2025 Baptr0b0t
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Medical.SuitSensor;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Medical.SuitSensors;

/// <summary>
///     Tracking device, embedded in almost all uniforms and jumpsuits.
///     If enabled, will report to crew monitoring console owners position and status.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(SuitSensorSystem))]
public sealed partial class SuitSensorComponent : Component
{
    // GoobStation - Start
    /// <summary>
    ///     Is this suit sensor for commands, BSO use only
    /// </summary>
    [DataField]
    public bool CommandTracker = false;
    // GoobStation - End
    /// <summary>
    ///     Choose a random sensor mode when item is spawned.
    /// </summary>
    [DataField("randomMode")]
    public bool RandomMode = true;

    /// <summary>
    ///     If true user can't change suit sensor mode
    /// </summary>
    [DataField("controlsLocked")]
    public bool ControlsLocked = false;

    /// <summary>
    ///  How much time it takes to change another player's sensors
    /// </summary>
    [DataField("sensorsTime")]
    public float SensorsTime = 1.75f;

    /// <summary>
    ///     Current sensor mode. Can be switched by user verbs.
    /// </summary>
    [DataField("mode")]
    public SuitSensorMode Mode = SuitSensorMode.SensorOff;

    /// <summary>
    ///     Activate sensor if user wear it in this slot.
    /// </summary>
    [DataField("activationSlot")]
    public string ActivationSlot = "jumpsuit";

    /// <summary>
    /// Activate sensor if user has this in a sensor-compatible container.
    /// </summary>
    [DataField("activationContainer")]
    public string? ActivationContainer;

    /// <summary>
    ///     How often does sensor update its owners status (in seconds). Limited by the system update rate.
    /// </summary>
    [DataField("updateRate")]
    public TimeSpan UpdateRate = TimeSpan.FromSeconds(2f);

    /// <summary>
    ///     Current user that wears suit sensor. Null if nobody wearing it.
    /// </summary>
    [ViewVariables]
    public EntityUid? User = null;

    /// <summary>
    ///     Next time when sensor updated owners status
    /// </summary>
    [DataField("nextUpdate", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    /// <summary>
    ///     The station this suit sensor belongs to. If it's null the suit didn't spawn on a station and the sensor doesn't work.
    /// </summary>
    [DataField("station")]
    public EntityUid? StationId = null;

    /// <summary>
    ///     The server the suit sensor sends it state to.
    ///     The suit sensor will try connecting to a new server when no server is connected.
    ///     It does this by calling the servers entity system for performance reasons.
    /// </summary>
    [DataField("server")]
    public string? ConnectedServer = null;

    /// <summary>
    /// The previous mode of the suit. This is used to restore the state when an EMP effect ends.
    /// </summary>
    [DataField, ViewVariables]
    public SuitSensorMode PreviousMode = SuitSensorMode.SensorOff;

    /// <summary>
    ///  The previous locked status of the controls.  This is used to restore the state when an EMP effect ends.
    ///  This keeps prisoner jumpsuits/internal implants from becoming unlocked after an EMP.
    /// </summary>
    [DataField, ViewVariables]
    public bool PreviousControlsLocked = false;
}

[ByRefEvent]
public record struct SuitSensorsSendAttemptEvent
{
    public bool Cancelled;
};
