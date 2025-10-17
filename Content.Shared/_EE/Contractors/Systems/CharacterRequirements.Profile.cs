// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared._EE.Contractors.Prototypes;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Mind;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

/// <summary>
///     Requires the profile to have one of a list of nationalities
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterNationalityRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public HashSet<ProtoId<NationalityPrototype>> Nationalities;

    const string RequirementColor = "green";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var localeString = "character-nationality-requirement";
        var names = Nationalities.Select(s => Loc.GetString(prototypeManager.Index(s).NameKey));
        var joinedNamed = string.Join($"[/color], [color={RequirementColor}]", names);
        var nationalityList = $"[color={RequirementColor}]{joinedNamed}[/color]";

        return Loc.GetString(localeString,
            ("inverted", Inverted),
            ("nationality", nationalityList));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile != null
            && Nationalities.Any(o => o == context.Profile.Nationality);
    }
}

/// <summary>
///     Requires the profile to have one of a list of employers
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterEmployerRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public HashSet<ProtoId<EmployerPrototype>> Employers;

    const string RequirementColor = "green";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var localeString = "character-employer-requirement";
        var names = Employers.Select(s => Loc.GetString(prototypeManager.Index(s).NameKey));
        var joinedNamed = string.Join($"[/color], [color={RequirementColor}]", names);
        var employerList = $"[color={RequirementColor}]{joinedNamed}[/color]";

        return Loc.GetString(localeString,
            ("inverted", Inverted),
            ("employers", employerList));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile != null
            && Employers.Any(o => o == context.Profile.Employer);
    }
}

/// <summary>
///     Requires the profile to have one of a list of lifepaths
/// </summary>
[UsedImplicitly, Serializable, NetSerializable]
public sealed partial class CharacterLifepathRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public HashSet<ProtoId<LifepathPrototype>> Lifepaths;

    const string RequirementColor = "green";

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Profile is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var localeString = "character-lifepath-requirement";
        var names = Lifepaths.Select(s => Loc.GetString(prototypeManager.Index(s).NameKey));
        var joinedNamed = string.Join($"[/color], [color={RequirementColor}]", names);
        var lifepathList = $"[color={RequirementColor}]{joinedNamed}[/color]";

        return Loc.GetString(localeString,
            ("inverted", Inverted),
            ("lifepaths", lifepathList));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.Profile != null
            && Lifepaths.Any(o => o == context.Profile.Lifepath);
    }
}
