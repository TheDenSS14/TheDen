// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 SlamBamActionman
// SPDX-FileCopyrightText: 2025 Blitz
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Shuttles.Components;
using Content.Shared.Procedural;
using Content.Shared.Salvage.Expeditions;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
using Content.Server.Salvage.Components;
using Content.Server.Salvage.Magnet;

namespace Content.Server.Salvage;

public sealed partial class SalvageSystem
{
    [ValidatePrototypeId<EntityPrototype>]
    public const string CoordinatesDisk = "CoordinatesDisk";

    private void OnSalvageClaimMessage(EntityUid uid, SalvageExpeditionConsoleComponent component, ClaimSalvageMessage args)
    {
        var station = _station.GetOwningStation(uid);

        if (TryComp<SalvageLastStationComponent>(uid, out var prevStation))
        {
            if (station != null)
                prevStation.StationID = (EntityUid) station;
            else if (station == null)
                station = prevStation.StationID;
        }

        if (prevStation != null && prevStation.StationID == null && station == null)
        {
            var stationQuery = EntityQueryEnumerator<SalvageMagnetDataComponent>();
            while (stationQuery.MoveNext(out var foundStation, out var salvageMagnetData))
            {
                station = foundStation;
                prevStation.StationID = (EntityUid) station;
            }
        }

        if (!TryComp<SalvageExpeditionDataComponent>(station, out var data) || data.Claimed)
            return;

        if (!data.Missions.TryGetValue(args.Index, out var missionparams))
            return;

        var cdUid = Spawn(CoordinatesDisk, Transform(uid).Coordinates);
        SpawnMission(missionparams, station.Value, cdUid);

        data.ActiveMission = args.Index;
        var mission = GetMission(_prototypeManager.Index<SalvageDifficultyPrototype>(missionparams.Difficulty), missionparams.Seed);
        data.NextOffer = _timing.CurTime + mission.Duration + TimeSpan.FromSeconds(1);

        _labelSystem.Label(cdUid, GetFTLName(_prototypeManager.Index<LocalizedDatasetPrototype>("NamesBorer"), missionparams.Seed));
        _audio.PlayPvs(component.PrintSound, uid);

        UpdateConsoles((station.Value, data));
    }

    private void OnSalvageConsoleInit(Entity<SalvageExpeditionConsoleComponent> console, ref ComponentInit args)
    {
        UpdateConsole(console);
    }

    private void OnSalvageConsoleParent(Entity<SalvageExpeditionConsoleComponent> console, ref EntParentChangedMessage args)
    {
        UpdateConsole(console);
    }

    private void UpdateConsoles(Entity<SalvageExpeditionDataComponent> component)
    {
        var state = GetState(component);

        var query = AllEntityQuery<SalvageExpeditionConsoleComponent, UserInterfaceComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var uiComp, out var xform))
        {
            var station = _station.GetOwningStation(uid, xform);

            if (TryComp<SalvageLastStationComponent>(component.Owner, out var prevStation))
            {
                if (station != null)
                    prevStation.StationID = (EntityUid) station;
                else if (station == null)
                    station = prevStation.StationID;
            }

            if (prevStation != null && prevStation.StationID == null && station == null)
            {
                var stationQuery = EntityQueryEnumerator<SalvageMagnetDataComponent>();
                while (stationQuery.MoveNext(out var foundStation, out var salvageMagnetData))
                {
                    station = foundStation;
                    prevStation.StationID = (EntityUid) station;
                }
            }

            //if (station != component.Owner)
            //    continue;

            _ui.SetUiState((uid, uiComp), SalvageConsoleUiKey.Expedition, state);
        }
    }

    private void UpdateConsole(Entity<SalvageExpeditionConsoleComponent> component)
    {
        var station = _station.GetOwningStation(component);
        SalvageExpeditionConsoleState state;

        if (TryComp<SalvageLastStationComponent>(component.Owner, out var prevStation))
        {
            if (station != null)
                prevStation.StationID = (EntityUid) station;
            else if (station == null)
                station = prevStation.StationID;
        }

        if (prevStation != null && prevStation.StationID == null && station == null)
        {
            var stationQuery = EntityQueryEnumerator<SalvageMagnetDataComponent>();
            while (stationQuery.MoveNext(out var foundStation, out var salvageMagnetData))
            {
                station = foundStation;
                prevStation.StationID = (EntityUid) station;
            }
        }

        if (TryComp<SalvageExpeditionDataComponent>(station, out var dataComponent))
        {
            state = GetState(dataComponent);
        }
        else
        {
            state = new SalvageExpeditionConsoleState(TimeSpan.Zero, false, true, 0, new List<SalvageMissionParams>());
        }

        _ui.SetUiState(component.Owner, SalvageConsoleUiKey.Expedition, state);
    }
}
