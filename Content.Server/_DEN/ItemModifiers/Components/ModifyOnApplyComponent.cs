using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;


namespace Content.Server._DEN.ItemModifiers.Components;

// What BluespacePlushiePatch wishes it could be.
/// <summary>
///     Applies and/or removes a set of components from an entity matching a whitelist/blacklist and tag filter.
/// </summary>
[RegisterComponent]
public sealed partial class ModifyOnApplyComponent : Component
{
    /// <summary>
    /// The amount of time it takes to apply the entity to its target.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float UseTime = 3f;

    /// <summary>
    /// The set of tags that must be matched on the entity in order for this to apply.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<TagPrototype>>? WhitelistTags;

    /// <summary>
    /// The set of tags that must not be matched on the entity in order for this to apply.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<TagPrototype>>? BlacklistTags;

    /// <summary>
    /// Components that are required to be on the entity this is being applied to for the application to occur.
    /// </summary>
    [DataField(customTypeSerializer: typeof(CustomHashSetSerializer<string, ComponentNameSerializer>))]
    public HashSet<string>? WhitelistComps;

    /// <summary>
    /// Components that must not be on the entity this is being applied to for the application to occur.
    /// </summary>
    [DataField(customTypeSerializer: typeof(CustomHashSetSerializer<string, ComponentNameSerializer>))]
    public HashSet<string>? BlacklistComps;

    /// <summary>
    /// Components to remove from the entity when this is applied.
    /// </summary>
    [DataField(customTypeSerializer: typeof(CustomHashSetSerializer<string, ComponentNameSerializer>))]
    public HashSet<string>? RemoveComps;

    /// <summary>
    /// Components to add to the entity this is being applied to.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry ApplyComps;

    /// <summary>
    /// The verb to display in the verb menu.
    /// </summary>
    [DataField]
    public LocId ApplyVerb = "modify-on-apply-verb-name";

    /// <summary>
    /// The message to display when the item is tried to apply to something invalid. Passes target and source.
    /// </summary>
    [DataField]
    public LocId InvalidMessage = "modify-on-apply-invalid-message";

    /// <summary>
    /// The message displayed to the user after applying this item to another one. Passes target and source.
    /// </summary>
    [DataField]
    public LocId PostApplyMessage = "modify-on-apply-post-apply-message";

    /// <summary>
    /// Optionally modifies the name of the item with this string. Passes target name.
    /// </summary>
    [DataField]
    public LocId? ModifyName;
}
