// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Administration.Managers;
using Content.Shared.Chat;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Customization.CharacterRequirements;

/// <summary>
///     Helper class, requires the player to not be role-banned from a list of given roles.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public abstract partial class CharacterRoleBanRequirement : CharacterRequirement
{
    [DataField]
    public HashSet<string> Roles;

    protected virtual LocId RequirementText => "character-requirement-role-ban";
    protected virtual string Prefix => string.Empty;

    private HashSet<string> PrefixedRoles => Roles
        .Select(r => Prefix + r)
        .ToHashSet();

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Player is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var roleNames = GetRoleNames(entityManager, prototypeManager);
        var roleList = string.Join(", ", roleNames);

        return Loc.GetString(RequirementText,
            ("inverted", Inverted),
            ("roles", roleList));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var playtimeManager = IoCManager.Resolve<ISharedPlaytimeManager>();
        return !IsBanned(context, playtimeManager);
    }

    protected virtual bool IsBanned(CharacterRequirementContext context, ISharedPlaytimeManager playtimeManager)
    {
        if (context.Player == null || !playtimeManager.TryGetRoleBans(context.Player, out var bans))
            return false;

        return bans.Overlaps(PrefixedRoles);
    }

    protected abstract List<string> GetRoleNames(IEntityManager entityManager, IPrototypeManager prototypeManager);
}

/// <summary>
///     Requires the player to not be antag-banned from a given role.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterAntagBanRequirement : CharacterRoleBanRequirement
{
    protected override LocId RequirementText => "character-requirement-antag-ban";
    protected override string Prefix => "Antag:"; // TODO: Role prefixes should be static and shared somewhere

    private static string _requirementColor = Color.Red.ToHex();

    protected override List<string> GetRoleNames(IEntityManager entityManager, IPrototypeManager prototypeManager)
    {
        var names = new List<string>();

        foreach (var role in Roles)
        {
            if (!prototypeManager.TryIndex<AntagPrototype>(role, out var antag))
                continue;

            var name = Loc.GetString(antag.Name);
            names.Add($"[color={_requirementColor}]{name}[/color]");
        }

        return names;
    }
}

/// <summary>
///     Requires the player to not be job-banned from a given role.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterJobBanRequirement : CharacterRoleBanRequirement
{
    protected override LocId RequirementText => "character-requirement-job-ban";
    protected override string Prefix => "Job:"; // TODO: Role prefixes should be static and shared somewhere

    protected override List<string> GetRoleNames(IEntityManager entityManager, IPrototypeManager prototypeManager)
    {
        var jobSystem = entityManager.System<SharedJobSystem>();
        var names = new List<string>();

        foreach (var role in Roles)
        {
            if (!prototypeManager.TryIndex<JobPrototype>(role, out var job))
                continue;

            var name = job.LocalizedName;
            var color = GetJobColor(role, jobSystem);
            names.Add($"[color={color.ToHex()}]{name}[/color]");
        }

        return names;
    }

    private static Color GetJobColor(string jobProto, SharedJobSystem jobSystem)
    {
        if (!jobSystem.TryGetPrimaryDepartment(jobProto, out var department) &&
            !jobSystem.TryGetDepartment(jobProto, out department))
            return Color.White;

        return department.Color;
    }
}
