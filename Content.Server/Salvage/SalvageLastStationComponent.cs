namespace Content.Server.Salvage.Components;

[RegisterComponent]
public sealed partial class SalvageLastStationComponent : Component
{
    [AutoNetworkedField]
    public EntityUid StationID;
}
