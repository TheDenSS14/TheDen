using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Overlays;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public abstract partial class BaseEquipmentHudComponent : Component
{
    /// <summary>
    ///     Action used to control which elements of the HUD to show.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId Action = "ActionControlHud";

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;
}