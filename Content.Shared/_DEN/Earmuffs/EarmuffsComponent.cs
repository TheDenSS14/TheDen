namespace Content.Shared._DEN.Earmuffs;


/// <summary>
/// This is used for only hearing within a certain distance.
/// </summary>
[RegisterComponent]
public sealed partial class EarmuffsComponent : Component
{
    [DataField]
    public int HearRange { get; set; }
}
