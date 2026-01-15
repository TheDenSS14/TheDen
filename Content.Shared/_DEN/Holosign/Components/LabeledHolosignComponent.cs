using Content.Shared._DEN.Holosign.Systems;
using Robust.Shared.GameStates;


namespace Content.Shared._DEN.Holosign.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedLabelableHolosignProjectorSystem))]
public sealed partial class LabeledHolosignComponent : Component
{
    [AutoNetworkedField]
    public string Description;

    [AutoNetworkedField]
    public bool IsNSFW;
}
