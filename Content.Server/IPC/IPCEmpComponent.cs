namespace Content.Server.Ipc;

[RegisterComponent]
public sealed partial class IpcEmpComponent : Component
{
    [DataField]
    public float DamageMultiplier = 1f;
    [DataField]
    public float BlindMultiplier = 1f;
    [DataField]
    public float StatusMultiplier = 1f;
    [DataField]
    public float StunMultiplier = 1f;

}
