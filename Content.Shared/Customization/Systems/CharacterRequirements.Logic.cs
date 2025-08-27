// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 Rosycup
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text;
using Content.Shared.Customization.Systems._DEN;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

// Fuck it we ball
/// <summary>
///     A helper class with some common functionality for logic requirements - requirements that have a list of other
///     requirements inside them, and evaluate success based on how many requirements pass. Logic requirements always
///     pass pre-checks (as they are a wrapper around other requirements with more particular bounds,) and the reason
///     for a logic requirement is always shown as a list of its sub-requirements;
/// </summary>
[Serializable, NetSerializable]
public abstract partial class CharacterLogicRequirement : CharacterRequirement
{
    [DataField]
    public List<CharacterRequirement> Requirements { get; private set; } = new();

    protected virtual LocId ListPrefix => "character-logic-and-requirement-listprefix";
    protected virtual LocId RequirementId => "character-logic-and-requirement";

    // Always true, because logical requirements do not inherently require a precheck
    public override bool PreCheckMandatory(CharacterRequirementContext context) => true;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var depth = context.Depth ?? 0;
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();
        var deeperContext = context.WithDepth(depth + 1);
        var reasons = characterRequirements.GetReasons(Requirements,
            deeperContext,
            entityManager,
            prototypeManager,
            configManager);

        if (reasons.Count == 0)
            return null;

        var reasonBuilder = new StringBuilder();
        foreach (var message in reasons)
        {
            var indent = new string(' ', depth * 2);
            var listPrefix = Loc.GetString(ListPrefix, ("indent", indent));
            reasonBuilder.Append(listPrefix + message);
        }

        return Loc.GetString(RequirementId,
            ("inverted", Inverted),
            ("options", reasonBuilder.ToString()));
    }
}

/// <summary>
///    Requires all of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicAndRequirement : CharacterLogicRequirement
{
    protected override LocId ListPrefix => "character-logic-and-requirement-listprefix";
    protected override LocId RequirementId => "character-logic-and-requirement";

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var depth = context.Depth ?? 0;
        var deeperContext = context.WithDepth(depth + 1);
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();

        return characterRequirements.CheckRequirementsValid(Requirements,
            deeperContext,
            entityManager,
            prototypeManager,
            configManager);
    }
}

/// <summary>
///     Requires any of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicOrRequirement : CharacterLogicRequirement
{
    protected override LocId ListPrefix => "character-logic-or-requirement-listprefix";
    protected override LocId RequirementId => "character-logic-or-requirement";

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var depth = context.Depth ?? 0;
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();
        var deeperContext = context.WithDepth(depth + 1);

        foreach (var requirement in Requirements)
        {
            if (characterRequirements.CheckRequirementValid(requirement,
                deeperContext,
                entityManager,
                prototypeManager,
                configManager))
                return true;
        }

        return false;
    }
}

/// <summary>
///     Requires only one of the requirements to be true
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterLogicXorRequirement : CharacterLogicRequirement
{
    protected override LocId ListPrefix => "character-logic-xor-requirement-listprefix";
    protected override LocId RequirementId => "character-logic-xor-requirement";

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var depth = context.Depth ?? 0;
        var succeeded = false;
        var characterRequirements = entityManager.EntitySysManager.GetEntitySystem<CharacterRequirementsSystem>();
        var deeperContext = context.WithDepth(depth + 1);

        foreach (var requirement in Requirements)
        {
            // We ignore non-manditory requirements that don't pass the pre-check,
            // because they should not count as "successful" - they should have no bearing on this.
            // This is because if they auto-succeeded it would violate the logic of "only one should succeed"
            if (!requirement.PreCheckMandatory(context) && !requirement.Mandatory)
                continue;

            if (characterRequirements.CheckRequirementValid(requirement,
                deeperContext,
                entityManager,
                prototypeManager,
                configManager))
            {
                if (succeeded) // If more than one requirement succeeds
                    return false;

                succeeded = true;
            }
        }

        return succeeded;
    }
}
