// SPDX-FileCopyrightText: 2025 Nemanja
// SPDX-FileCopyrightText: 2025 Sir Warock
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Content.Shared.Localizations;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.EntityEffects.Effects;

/// <summary>
/// Version of <see cref="HealthChange"/> that distributes the healing to groups
/// </summary>
[UsedImplicitly]
public sealed partial class EvenHealthChange : EntityEffect
{
    /// <summary>
    /// Damage to heal, collected into entire damage groups.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<ProtoId<DamageGroupPrototype>, FixedPoint2> Damage = new();

    /// <summary>
    /// Should this effect scale the damage by the amount of chemical in the solution?
    /// Useful for touch reactions, like styptic powder or acid.
    /// Only usable if the EntityEffectBaseArgs is an EntityEffectReagentArgs.
    /// </summary>
    [DataField]
    public bool ScaleByQuantity;

    /// <summary>
    /// Should this effect ignore damage modifiers?
    /// </summary>
    [DataField]
    public bool IgnoreResistances = true;

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var damages = new List<string>();
        var heals = false;
        var deals = false;

        foreach (var (group, amount) in Damage)
        {
            var groupProto = prototype.Index(group);

            var sign = FixedPoint2.Sign(amount);
            var mod = 1f;

            if (sign < 0)
            {
                heals = true;
            }
            else if (sign > 0)
            {
                deals = true;
            }

            damages.Add(
                Loc.GetString("health-change-display",
                    ("kind", groupProto.LocalizedName),
                    ("amount", MathF.Abs(amount.Float() * mod)),
                    ("deltasign", sign)
                ));
        }

        var healsordeals = heals ? (deals ? "both" : "heals") : (deals ? "deals" : "none");
        return Loc.GetString("reagent-effect-guidebook-even-health-change",
            ("chance", Probability),
            ("changes", ContentLocalizationManager.FormatList(damages)),
            ("healsordeals", healsordeals));
    }

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (!args.EntityManager.TryGetComponent<DamageableComponent>(args.TargetEntity, out var damageable))
            return;

        var protoMan = IoCManager.Resolve<IPrototypeManager>();

        var scale = FixedPoint2.New(1);

        if (args is EntityEffectReagentArgs reagentArgs)
        {
            scale = ScaleByQuantity ? reagentArgs.Quantity * reagentArgs.Scale : reagentArgs.Scale;
        }

        var damagableSystem = args.EntityManager.System<DamageableSystem>();

        var dspec = new DamageSpecifier();

        foreach (var (group, amount) in Damage)
        {
            var groupProto = protoMan.Index(group);
            var groupDamage = new Dictionary<string, FixedPoint2>();
            foreach (var damageId in groupProto.DamageTypes)
            {
                var damageAmount = damageable.Damage.DamageDict.GetValueOrDefault(damageId);
                if (damageAmount != FixedPoint2.Zero)
                    groupDamage.Add(damageId, damageAmount);
            }

            var sum = groupDamage.Values.Sum();
            foreach (var (damageId, damageAmount) in groupDamage)
            {
                var existing = dspec.DamageDict.GetOrNew(damageId);
                dspec.DamageDict[damageId] = existing + damageAmount / sum * amount;
            }
        }
        
        damagableSystem.TryChangeDamage(
            args.TargetEntity,
            dspec * scale,
            IgnoreResistances,
            interruptsDoAfters: false);
    }
}
