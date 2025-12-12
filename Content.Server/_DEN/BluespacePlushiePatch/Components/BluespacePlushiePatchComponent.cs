using Content.Shared.Item;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;


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

    /// <summary>
    /// The size of the resulting inventory after applying the patch.
    /// </summary>
    [DataField]
    public List<Box2i> InventorySize = new() { new(0, 0, 5, 4) };

    /// <summary>
    /// The maximum size of items that can be put in the inventory.
    /// </summary>
    [DataField]
    public ProtoId<ItemSizePrototype> MaxItemSize = "Huge";

    /// <summary>
    /// The shape the plushie becomes.
    /// </summary>
    [DataField]
    public List<Box2i> ItemShape = new() {  new(0, 0, 3, 3) };

    /// <summary>
    /// The item size that the plushie becomes.
    /// </summary>
    [DataField]
    public ProtoId<ItemSizePrototype> PlushieItemSize = "Ginormous";

    /// <summary>
    /// Components to remove from the target when transforming it.
    /// </summary>
    [DataField(customTypeSerializer:typeof(CustomHashSetSerializer<string, ComponentNameSerializer>))]
    public HashSet<string>? RemoveComps;
}
