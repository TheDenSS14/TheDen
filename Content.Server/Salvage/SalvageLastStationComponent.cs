namespace Content.Server.Salvage.Components;

[RegisterComponent]
public sealed partial class SalvageLastStation : Component
{
    [AutoNetworkedField]
    public EntityUid StationID;
}
