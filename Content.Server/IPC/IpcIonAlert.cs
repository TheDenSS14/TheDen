namespace Content.Server.Ipc;

[RegisterComponent]
public sealed partial class IpcIonAlertComponent : Component
{
    [DataField]
    public float AlertChance = 0.9f; // sneaky chance to miss an ion storm
}
