namespace Content.Server._DEN.Research.Components;


/// <summary>
/// This is used for keeping track of soft cap multiplier.
/// </summary>
[RegisterComponent]
public sealed partial class StationResearchRecordComponent : Component
{
    [DataField]
    public float SoftCapMultiplier = 1f;
}
