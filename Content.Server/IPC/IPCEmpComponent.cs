namespace Content.Server.Ipc;

[RegisterComponent]
public sealed partial class IpcEmpComponent : Component
{
    [DataField]
    public bool Disabled = false;

    [DataField]
    public float DurationMultiplier = 1f;
}
