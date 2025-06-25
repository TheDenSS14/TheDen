// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Reagent;
using Content.Server.Psionics;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.ReagentEffects
{
    /// <summary>
    /// Rerolls psionics once.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class ChemRerollPsionic : EntityEffect
    {
        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-chem-reroll-psionic", ("chance", Probability));

        /// <summary>
        /// Reroll multiplier.
        /// </summary>
        [DataField("bonusMultiplier")]
        public float BonusMuliplier = 1f;

        public override void Effect(EntityEffectBaseArgs args)
        {
            if (args is not EntityEffectReagentArgs _)
                return;

            var psySys = args.EntityManager.EntitySysManager.GetEntitySystem<PsionicsSystem>();
            psySys.RerollPsionics(args.TargetEntity, bonusMuliplier: BonusMuliplier);
        }
    }
}
