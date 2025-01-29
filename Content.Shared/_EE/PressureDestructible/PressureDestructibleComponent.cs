namespace Content.Shared._EE.PressureDestructible;


/// <summary>
///     This is used in making pressure destroy structures.
/// </summary>
[RegisterComponent]
public sealed partial class PressureDestructibleComponent : Component
{
    /// <summary>
    ///     How much pressure could this object reasonably withstand?
    /// </summary>
    [DataField]
    public float MaxWithstandingPressure { get; set; }

    /// <summary>
    ///     If it has a grille below it, what should we multiply its ability to withstand pressure by?
    /// </summary>
    [DataField]
    public float GrilleStabilityMultiplier { get; set; }
}
