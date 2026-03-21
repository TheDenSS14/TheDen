// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 LankLTE
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2024 SlamBamActionman
// SPDX-FileCopyrightText: 2024 sleepyyapril
// SPDX-FileCopyrightText: 2025 Terkala
// SPDX-FileCopyrightText: 2025 taydeo
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Zombies;
using Content.Shared.EntityEffects;
using Content.Shared.Zombies;
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

        // Cure tumor infection if in early stages (before tumor fully forms)
        if (entityManager.TryGetComponent<ZombieTumorInfectionComponent>(args.TargetEntity, out var infection))
        {
            // Only cure if no tumor has formed yet (Incubation or Early stage)
            // Once the tumor is formed (TumorFormed or Advanced), it requires surgery
            if (infection.Stage == ZombieTumorInfectionStage.Incubation ||
                infection.Stage == ZombieTumorInfectionStage.Early)
            {
                entityManager.RemoveComponent<ZombieTumorInfectionComponent>(args.TargetEntity);
            }
        }

        if (Innoculate)
        {
            entityManager.EnsureComponent<ZombieImmuneComponent>(args.TargetEntity);
            // Also make immune to tumor infections
            entityManager.EnsureComponent<ZombieTumorImmuneComponent>(args.TargetEntity);
        }
    }
}

