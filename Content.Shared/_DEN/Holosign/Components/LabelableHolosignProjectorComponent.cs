using Content.Shared._DEN.Holosign.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Holosign.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedLabelableHolosignProjectorSystem))]
public sealed partial class LabelableHolosignProjectorComponent : Component
{
    /// <summary>
    /// The entity to spawn with this projector.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId SignProto;

    [DataField]
    public EntityWhitelist SignWhitelist;

    [DataField]
    public bool UsesCharges = false;

    [ViewVariables(VVAccess.ReadWrite), Access(Other = AccessPermissions.ReadWriteExecute)]
    [DataField]
    public string BarrierDescription = string.Empty;

    /// <summary>
    /// The maximum length of a description that can be attached to a barrier.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public int MaxDescriptionChars = 512;

    [DataField(required: true)]
    public bool IsNSFW;
}

[Serializable, NetSerializable]
public sealed class LabelableHolosignProjectorComponentState(string barrierDescription) : IComponentState
{
    public string BarrierDescription = barrierDescription;

    public int MaxDescriptionChars;
}
