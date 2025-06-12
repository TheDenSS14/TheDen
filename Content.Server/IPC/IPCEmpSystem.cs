using Content.Server.Emp;
using Content.Server.Flash;
using Content.Server.Ipc;
using Content.Server.Stunnable;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.IpcEmp;

internal sealed class IpcEmpSystem : EntitySystem
{
    [Dependency]
    private readonly StunSystem _stun = default!;
    [Dependency]
    private readonly FlashSystem _flash = default!;
    [Dependency]
    private readonly DamageableSystem _damageable = default!;
    [Dependency]
    private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IpcEmpComponent, EmpPulseEvent>(OnEmpPulse);
    }

    private void OnEmpPulse(EntityUid uid, IpcEmpComponent ipcEnt, ref EmpPulseEvent ev)
    {
        ev.Affected = true;
        ev.Disabled = true;

        var empDamage = ev.EnergyConsumption * ipcEnt.DamageMultiplier; // Using how much energy is consumed to scale the damage (2700000 is an EMP grenade)
        DamageSpecifier damage = new(_prototypeManager.Index<DamageTypePrototype>("Shock"), empDamage);
        _damageable.TryChangeDamage(uid, damage);
    }
}
