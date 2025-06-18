using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.Queries.Queries;

public sealed partial class TagFilter : UtilityQueryFilter
{
    /// <summary>
    /// Tags to filter for.
    /// </summary>
    [DataField("tags", required: true)]
    public HashSet<ProtoId<TagPrototype>> Tags = new();
}
