using Content.Server.IPCEmp;

namespace Content.Server.IPC;

[RegisterComponent]
[Access(typeof(IPCEmpSystem))]
public sealed partial class IPCEmpComponent : Component
{
    [DataField("duration")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int EmpDuration { get; set; } = 5000;
}

