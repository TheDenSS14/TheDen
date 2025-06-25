// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class ModifyBloodLevel : EntityEffect
{
    [DataField]
    public bool Scaled = false;

    [DataField]
    public FixedPoint2 Amount = 1.0f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-modify-blood-level", ("chance", Probability),
            ("deltasign", MathF.Sign(Amount.Float())));

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args.EntityManager.TryGetComponent<BloodstreamComponent>(args.TargetEntity, out var blood))
        {
            var sys = args.EntityManager.System<BloodstreamSystem>();
            var amt = Amount;
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                if (Scaled)
                    amt *= reagentArgs.Quantity;
                amt *= reagentArgs.Scale;
            }

            sys.TryModifyBloodLevel(args.TargetEntity, amt, blood);
        }
    }
}
