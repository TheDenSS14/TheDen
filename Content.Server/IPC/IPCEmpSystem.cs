using Content.Server.Emp;
using Content.Server.Ipc;
using Robust.Server.GameObjects;

namespace Content.Server.IpcEmp;

public sealed class IpcEmpSystem : EntitySystem
{
    [Dependency]
    private readonly TransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IpcEmpComponent, EmpPulseEvent>(OnEmpPulse);
        SubscribeLocalEvent<IpcEmpComponent, EmpDisabledRemoved>(OnEmpDisabledRemoved);
    }

    private void OnEmpPulse(Entity<IpcEmpComponent> ipcEnt, ref EmpPulseEvent ev)
    {
        if (!ipcEnt.Comp.Disabled)
        {
            ev.Affected = true;
            ev.Disabled = true;
            ipcEnt.Comp.Disabled = true;
        }
    }

    private void OnEmpDisabledRemoved(Entity<IpcEmpComponent> ipcEnt, ref EmpDisabledRemoved ev)
    {
        if (ipcEnt.Comp.Disabled)
        {
            ipcEnt.Comp.Disabled = false;
        }
    }
}
