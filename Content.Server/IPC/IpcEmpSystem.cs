using Content.Server.Emp;
using Content.Server.Ipc;
using Content.Server.Stunnable;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Jittering;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Robust.Shared.Prototypes;

namespace Content.Server.IpcEmp;

internal sealed class IpcEmpSystem : EntitySystem
{
    [Dependency]
    private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency]
    private readonly StunSystem _stun = default!;
    [Dependency]
    private readonly DamageableSystem _damageable = default!;
    [Dependency]
    private readonly SharedJitteringSystem _jitter = default!;
    [Dependency]
    private readonly StatusEffectsSystem _status = default!;
    [Dependency]
    private readonly SharedStutteringSystem _stutter = default!;

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

        var empBlind = ev.Duration * ipcEnt.BlindMultiplier;
        _status.TryAddStatusEffect(uid, TemporaryBlindnessSystem.BlindingStatusEffect, empBlind, true, TemporaryBlindnessSystem.BlindingStatusEffect);

        var empStatus = ev.Duration * ipcEnt.StatusMultiplier;
        var empStatusFrequency = empDamage / 200f;
        _jitter.DoJitter(uid, empStatus, true, empDamage, empStatusFrequency, true);
        _stutter.DoStutter(uid, empStatus, true);

        var empStun = ev.Duration * ipcEnt.StunMultiplier;
        _stun.TryParalyze(uid, empStun, true);
    }
}
