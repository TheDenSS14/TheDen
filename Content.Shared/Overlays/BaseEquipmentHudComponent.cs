using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Overlays;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public abstract partial class BaseEquipmentHudComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;
}