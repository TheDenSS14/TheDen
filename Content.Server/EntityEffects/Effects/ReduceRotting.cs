// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 XavierSomething <tylernguyen203@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Content.Server.Atmos.Rotting;

namespace Content.Server.EntityEffects.Effects
{
    /// <summary>
    /// Reduces the rotting accumulator on the patient, making them revivable.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class ReduceRotting : EntityEffect
    {
        [DataField("seconds")]
        public double RottingAmount = 10;

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-reduce-rotting",
                ("chance", Probability),
                ("time", RottingAmount));
        public override void Effect(EntityEffectBaseArgs args)
        {
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                if (reagentArgs.Scale != 1f)
                    return;
            }

            var rottingSys = args.EntityManager.EntitySysManager.GetEntitySystem<RottingSystem>();

            rottingSys.ReduceAccumulator(args.TargetEntity, TimeSpan.FromSeconds(RottingAmount));
        }
    }
}
