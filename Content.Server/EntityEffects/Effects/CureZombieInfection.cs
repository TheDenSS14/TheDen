// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 LankLTE
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2024 SlamBamActionman
// SPDX-FileCopyrightText: 2024 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Zombies;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class CureZombieInfection : EntityEffect
{
    [DataField]
    public bool Innoculate;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        if(Innoculate)
            return Loc.GetString("reagent-effect-guidebook-innoculate-zombie-infection", ("chance", Probability));

        return Loc.GetString("reagent-effect-guidebook-cure-zombie-infection", ("chance", Probability));
    }

    // Removes the Zombie Infection Components
    public override void Effect(EntityEffectBaseArgs args)
    {
        var entityManager = args.EntityManager;
        if (entityManager.HasComponent<IncurableZombieComponent>(args.TargetEntity))
            return;

        entityManager.RemoveComponent<ZombifyOnDeathComponent>(args.TargetEntity);
        entityManager.RemoveComponent<PendingZombieComponent>(args.TargetEntity);

        if (Innoculate)
        {
            entityManager.EnsureComponent<ZombieImmuneComponent>(args.TargetEntity);
        }
    }
}

