using Content.Shared.Tag;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.ItemModifiers.Components;


/// <summary>
/// Marks an item as being able to have items with ModifyOnApplyComponents attached to them.
/// </summary>
[RegisterComponent]
public sealed partial class ModifiableComponent : Component
{
    [DataField, ViewVariables]
    public HashSet<ProtoId<TagPrototype>> Tags = new();
}
