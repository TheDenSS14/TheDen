using System.Linq;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Server.StationRecords;
using Content.Server.StationRecords.Systems;
using Content.Shared._DEN.StationRecords;
using Content.Shared.Access.Systems;
using Content.Shared.Roles;
using Content.Shared.StationRecords;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.StationRecords;


public sealed class StationJobsConsoleSystem : EntitySystem
{
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly AccessReaderSystem _access = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly StationJobsSystem _stationJobsSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly StationRecordsSystem _recordsSystem = default!;

    public override void Initialize()
    {
        Subs.BuiEvents<StationJobsConsoleComponent>(StationJobsConsoleKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(UpdateUserInterface);
            subs.Event<AdjustStationJobMsg>(OnAdjustJob);
        });
    }

    private void OnAdjustJob(Entity<StationJobsConsoleComponent> ent, ref AdjustStationJobMsg msg)
    {
        var stationUid = _station.GetOwningStation(ent);
        if (stationUid is EntityUid station)
        {
            if (TryComp(stationUid, out StationJobsComponent? stationJobs))
            {
                foreach (var job in stationJobs.JobList)
                {
                    if (!_proto.TryIndex<JobPrototype>(job.Key, out var jobproto))
                        continue;

                    if (!jobproto.AdjustableCount && job.Key == msg.JobProto)
                    {
                        UpdateUserInterface(ent);
                        return;
                    }
                }
            }
            _stationJobsSystem.TryAdjustJobSlot(station, msg.JobProto, msg.Amount, false, true);
            UpdateUserInterface(ent);
        }
    }
    private void UpdateUserInterface<T>(Entity<StationJobsConsoleComponent> ent, ref T args)
    {
        UpdateUserInterface(ent);
    }
    private void UpdateUserInterface(Entity<StationJobsConsoleComponent> ent)
    {
        var (uid, console) = ent;
        var owningStation = _station.GetOwningStation(uid);

        IReadOnlyDictionary<string, uint?>? jobList = null;
        IReadOnlyDictionary<string, uint?>? jobSlots = null;

        if (owningStation is not null)
        {
            jobList = _stationJobsSystem.GetJobs(owningStation.Value);
            jobSlots = GetTotalJobSlots(owningStation.Value);
        }

        if (jobList is null || jobSlots is null)
            return;

        StationJobsConsoleState newState = new(jobList, jobSlots);
        _ui.SetUiState(uid, StationJobsConsoleKey.Key, newState);
    }

    private IReadOnlyDictionary<string, uint?>? GetTotalJobSlots(EntityUid station)
    {
        var iter = _recordsSystem.GetRecordsOfType<GeneralStationRecord>(station);
        var jobSlots = _stationJobsSystem.GetJobs(station);

        var jobList = new List<(string, uint?)>();

        foreach (var record in iter)
        {
            if (!jobList.Select(a => a.Item1).Contains(record.Item2.JobPrototype))
                jobList.Add((record.Item2.JobPrototype, 1));
            else
            {
                var row = jobList.FirstOrDefault(a => a.Item1 == record.Item2.JobPrototype);
                row.Item2++;
            }
        }

        var slotsOut = new Dictionary<string, uint?>();

        foreach (var (k, v) in jobList)
            slotsOut[k] = v ?? 0;

        foreach (var (k, v) in jobSlots)
            slotsOut[k] = (slotsOut.TryGetValue(k, out var cur) ? cur : 0) + (v ?? 0);

        return slotsOut;
    }
}
