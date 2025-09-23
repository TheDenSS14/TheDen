// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Mind;
using Content.Shared.Mindshield.Components;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

/// <summary>
///     Requires the player to have a mindshield
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterMindshieldRequirement : CharacterRequirement
{
    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Entity is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString("character-mindshield-requirement", ("inverted", Inverted));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var mindSystem = entityManager.System<SharedMindSystem>();
        return context.Entity is not null
            && mindSystem.TryGetMind(context.Entity.Value, out var _, out var mindComponent)
            && entityManager.HasComponent<MindShieldComponent>(mindComponent.CurrentEntity);
    }
}
