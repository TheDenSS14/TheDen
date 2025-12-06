using Robust.Shared.GameStates;


namespace Content.Server._DEN.BluespacePlushiePatch.Components;


/// <summary>
///     Indicates that an item can have a BluespacePlushiePatch applied.
/// </summary>
[RegisterComponent]
public sealed partial class BluespacePlushiePatchableComponent : Component
{
    [DataField]
    public bool HasPatch = false;
}
