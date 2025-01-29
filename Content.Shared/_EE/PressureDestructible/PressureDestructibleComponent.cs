namespace Content.Shared._EE.PressureDestructible;


/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class PressureDestructibleComponent : Component
{
    [DataField]
    public float MaxWithstandingPressure { get; set; }

    [DataField]
    public float GrilleStabilityMultiplier { get; set; }
}
