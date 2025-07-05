// SPDX-FileCopyrightText: 2021 Alex Evgrashin
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2022 Acruid
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 Julian Giebel
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 chromiumboy
// SPDX-FileCopyrightText: 2023 keronshb
// SPDX-FileCopyrightText: 2023 themias
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 Aiden
// SPDX-FileCopyrightText: 2025 Baptr0b0t
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Ted Lukin
// SPDX-FileCopyrightText: 2025 pheenty
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.CrewMonitoring;
using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Power.Components; // TheDen
using Content.Server.Power.EntitySystems; // DeltaV
using Content.Server.PowerCell;
using Content.Shared.Medical.CrewMonitoring;
using Content.Shared.Medical.SuitSensor;
using Content.Shared.Pinpointer;
using Robust.Server.GameObjects;
using Robust.Shared.Audio; // DeltaV
using Robust.Shared.Audio.Systems; // DeltaV
using Robust.Shared.Timing; // DeltaV


namespace Content.Server.Medical.CrewMonitoring;

public sealed class CrewMonitoringConsoleSystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _cell = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!; // DeltaV
    [Dependency] private readonly IGameTiming _timing = default!; // DeltaV

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, DeviceNetworkPacketEvent>(OnPacketReceived);
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, BoundUIOpenedEvent>(OnUIOpened);
    }

    private void OnRemove(EntityUid uid, CrewMonitoringConsoleComponent component, ComponentRemove args)
    {
        component.ConnectedSensors.Clear();
    }

    private void OnPacketReceived(EntityUid uid, CrewMonitoringConsoleComponent component, DeviceNetworkPacketEvent args)
    {
        var payload = args.Data;

        // Check command
        if (!payload.TryGetValue(DeviceNetworkConstants.Command, out string? command))
            return;

        if (command != DeviceNetworkConstants.CmdUpdatedState)
            return;

        if (!payload.TryGetValue(SuitSensorConstants.NET_STATUS_COLLECTION, out Dictionary<string, SuitSensorStatus>? sensorStatus))
            return;
        component.ConnectedSensors = sensorStatus;

        UpdateUserInterface(uid, component);

        // DeltaV - start of alert system code
        if (!component.AlertsEnabled)
            return;

        // station power (for the machine version)
        if (this.HasComp<ApcPowerReceiverComponent>(uid) && !this.IsPowered(uid, EntityManager)) // TheDen - DeltaV does IsPowered differently
            return;

        // cell power (for the handheld)
        if (!_cell.HasActivatableCharge(uid))
            return;

        foreach (var (sensorId, status) in sensorStatus)
        {
            // DamagePercentage above 1f is considered critical. It is null when sensor vitals are off.
            var isCritical = status.DamagePercentage is >= 1f;

            // Skip crew members that we have already alerted about
            if (component.AlertedSensors.Contains(sensorId))
            {
                if (status.IsAlive && !isCritical)
                    component.AlertedSensors.Remove(sensorId);
                continue;
            }

            if (!status.IsAlive || isCritical)
            {
                if (_timing.CurTime >= component.NextAlert)
                {
                    var audioParams = AudioParams.Default.WithVolume(-2f).WithMaxDistance(4f);
                    _audio.PlayPvs(component.AlertSound, uid, audioParams);
                    component.NextAlert = _timing.CurTime + component.AlertCooldown;
                }

                // We are doing this outside the cooldown check to avoid "alert queues"
                // If two people die at the same time and remain dead for longer, we want to alert once for both people
                // instead of alerting once for the first one, waiting the cooldown, and then alerting again for the second one.
                component.AlertedSensors.Add(sensorId);
            }
        }
        // DeltaV - end of alert system code
    }

    private void OnUIOpened(EntityUid uid, CrewMonitoringConsoleComponent component, BoundUIOpenedEvent args)
    {
        if (!_cell.TryUseActivatableCharge(uid))
            return;

        UpdateUserInterface(uid, component);
    }

    private void UpdateUserInterface(EntityUid uid, CrewMonitoringConsoleComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!_uiSystem.IsUiOpen(uid, CrewMonitoringUIKey.Key))
            return;

        // The grid must have a NavMapComponent to visualize the map in the UI
        var xform = Transform(uid);

        if (xform.GridUid != null)
            EnsureComp<NavMapComponent>(xform.GridUid.Value);

        // Update all sensors info
        // GoobStation - Start
        var isCommandOnly = HasComp<CrewMonitorScanningComponent>(uid);

        var filteredSensors = component.ConnectedSensors
            .Where(pair => isCommandOnly
                ? pair.Value.IsCommandTracker
                : !pair.Value.IsCommandTracker)
            .Select(pair => pair.Value)
            .ToList();
        _uiSystem.SetUiState(uid, CrewMonitoringUIKey.Key, new CrewMonitoringState(filteredSensors));
        // GoobStation - End
        //var allSensors = component.ConnectedSensors.Values.ToList();
        //_uiSystem.SetUiState(uid, CrewMonitoringUIKey.Key, new CrewMonitoringState(allSensors));
    }
}
