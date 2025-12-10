using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.StationRecords;

[Serializable, NetSerializable]
public enum StationJobsConsoleKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class StationJobsConsoleState(IReadOnlyDictionary<string, uint?>? jobList, IReadOnlyDictionary<string, uint?>? jobSlots)
    : BoundUserInterfaceState
{
    public IReadOnlyDictionary<string, uint?>? JobList { get; set; } = jobList;

    public IReadOnlyDictionary<string, uint?>? JobSlots { get; set; } = jobSlots;

    public StationJobsConsoleState() : this(null, null) { }
}

[Serializable, NetSerializable]
public sealed class AdjustStationJobMsg(string jobProto, int amount) : BoundUserInterfaceMessage
{
    public string JobProto { get; } = jobProto;
    public int Amount { get; } = amount;
}
