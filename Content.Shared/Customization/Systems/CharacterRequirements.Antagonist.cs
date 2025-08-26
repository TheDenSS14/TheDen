// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Mind;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

/// <summary>
///     Requires the player to be a specific antagonist
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterAntagonistRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<AntagPrototype>> Antagonists;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Entity is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString("character-antagonist-requirement", ("inverted", Inverted));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var mindSystem = entityManager.System<SharedMindSystem>();
        if (context.Entity == null
            || !mindSystem.TryGetMind(context.Entity.Value, out var mind, out var mindComponent))
            return false;

        foreach (var mindRoleComponent in mindComponent.MindRoles
            .Select(entityManager.GetComponent<MindRoleComponent>))
        {
            if (!mindRoleComponent.AntagPrototype.HasValue)
                continue;

            if (Antagonists.Contains(mindRoleComponent.AntagPrototype.Value))
                return true;
        }

        return false;
    }
}
