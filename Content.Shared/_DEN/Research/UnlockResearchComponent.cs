using Content.Shared.Research.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Research;


/// <summary>
/// This is used for unlocking specific research on map initialization of the component.
/// </summary>
[RegisterComponent]
public sealed partial class UnlockResearchComponent : Component
{
    [DataField]
    public bool UnlockAll { get; set; }

    [DataField]
    public int StartingPoints { get; set; }

    [DataField]
    public HashSet<ProtoId<TechnologyPrototype>>? Technologies { get; set; }
}
