using Robust.Shared.Prototypes;


namespace Content.Server._DEN.BluespacePlushiePatch.Components;


/// <summary>
///     Handles Bluespace patch components.
/// </summary>
[RegisterComponent]
public sealed partial class BluespacePlushiePatchComponent : Component
{
    /// <summary>
    /// The amount of time it takes to apply the patch.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float PatchUseTime = 3f;
}
