using Content.Server.Emp;
using Content.Server.IPC;

namespace Content.Server.IPCEmp;

public sealed class IPCEmpSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<IPCEmpComponent, EmpPulseEvent>(OnEmpPulse);
    }

    private void OnEmpPulse(EntityUid uid, IPCEmpComponent component, ref EmpPulseEvent args)
    {

    }
}
