// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.CCVar;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

/// <summary>
///     Requires the selected job to be one of the specified jobs
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterJobRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<JobPrototype>> Jobs;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.SelectedJob is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var jobs = new List<string>();
        var depts = prototypeManager.EnumeratePrototypes<DepartmentPrototype>()
            .ToList()
            .OrderBy(d => Loc.GetString($"department-{d.ID}"));

        foreach (var j in Jobs)
        {
            var jobProto = prototypeManager.Index(j);
            var color = Color.LightBlue;
            foreach (var dept in depts)
            {
                if (dept.Roles.Contains(j))
                {
                    color = dept.Color;
                    break;
                }
            }

            var hexColor = color.ToHex();
            var jobName = Loc.GetString(jobProto.Name);
            jobs.Add($"[color={hexColor}]{jobName}[/color]");
        }

        return Loc.GetString("character-job-requirement",
            ("inverted", Inverted),
            ("jobs", string.Join(", ", jobs)));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.SelectedJob is not null
            && Jobs.Contains(context.SelectedJob.ID);
    }
}

/// <summary>
///     Requires the selected job to be in one of the specified departments
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterDepartmentRequirement : CharacterRequirement
{
    [DataField(required: true)]
    public List<ProtoId<DepartmentPrototype>> Departments;

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.SelectedJob is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var departments = new List<string>();
        foreach (var d in Departments)
        {
            var deptProto = prototypeManager.Index(d);
            var color = deptProto.Color;

            departments.Add($"[color={color.ToHex()}]{Loc.GetString($"department-{deptProto.ID}")}[/color]");
        }

        return Loc.GetString("character-department-requirement",
            ("inverted", Inverted),
            ("departments", string.Join(", ", departments)));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return context.SelectedJob is not null
            && Departments.Any(d => prototypeManager.Index(d).Roles.Contains(context.SelectedJob.ID));
    }
}

/// <summary>
///     A helper class with some common functions for checking if a contexts's playtime is within a specific set of
///     bounds. By default, this requirement will pretend that a context with null playtimes has zero playtime,
///     and playtime checking will be disabled if the game role timer CVAR is disabled.
/// </summary>
[Serializable, NetSerializable]
public abstract partial class CharacterTimeRequirement : CharacterRequirement
{
    [DataField]
    public TimeSpan Min = TimeSpan.MinValue;

    [DataField]
    public TimeSpan Max = TimeSpan.MaxValue;

    // Always allow checking, as having no provided playtimes is treated as equivalent of having 0.
    public override bool PreCheckMandatory(CharacterRequirementContext context) => true;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (!configManager.GetCVar(CCVars.GameRoleTimers))
            return null;

        var playtime = GetTotalPlaytime(context, prototypeManager);

        if (playtime <= Min)
            return GetMinimumText(playtime,
                context,
                entityManager,
                prototypeManager,
                configManager);

        if (playtime > Max)
            return GetMaximumText(playtime,
                context,
                entityManager,
                prototypeManager,
                configManager);

        return null;
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return !configManager.GetCVar(CCVars.GameRoleTimers)
            || InBounds(GetTotalPlaytime(context, prototypeManager));
    }

    protected bool InBounds(TimeSpan playtime)
    {
        return playtime >= Min && playtime < Max;
    }

    protected abstract TimeSpan GetTotalPlaytime(CharacterRequirementContext context,
        IPrototypeManager prototypeManager);

    protected abstract string? GetMinimumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager);

    protected abstract string? GetMaximumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager);
}

/// <summary>
///     Requires the playtime for a department to be within a certain range
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterDepartmentTimeRequirement : CharacterTimeRequirement
{
    [DataField(required: true)]
    public ProtoId<DepartmentPrototype> Department;

    protected override TimeSpan GetTotalPlaytime(CharacterRequirementContext context,
        IPrototypeManager prototypeManager)
    {
        if (context.Playtimes is null)
            return TimeSpan.Zero;

        var department = prototypeManager.Index(Department);
        var playtime = TimeSpan.Zero;

        foreach (var other in department.Roles)
        {
            var proto = prototypeManager.Index(other).PlayTimeTracker;
            context.Playtimes.TryGetValue(proto, out var otherTime);
            playtime += otherTime;
        }

        return playtime;
    }

    protected override string? GetMinimumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var department = prototypeManager.Index(Department);
        return Inverted
            ? null
            : Loc.GetString("character-timer-department-insufficient",
                ("time", Min.TotalMinutes - playtime.TotalMinutes),
                ("department", Loc.GetString($"department-{department.ID}")),
                ("departmentColor", department.Color));
    }

    protected override string? GetMaximumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var department = prototypeManager.Index(Department);
        return Inverted
            ? null
            : Loc.GetString("character-timer-department-too-high",
                ("time", playtime.TotalMinutes - Max.TotalMinutes),
                ("department", Loc.GetString($"department-{department.ID}")),
                ("departmentColor", department.Color));
    }
}

/// <summary>
///     Requires the player to have a certain amount of overall job time
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterOverallTimeRequirement : CharacterTimeRequirement
{
    protected override string? GetMinimumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Inverted
            ? null
            : Loc.GetString("character-timer-overall-insufficient",
                ("time", Min.TotalMinutes - playtime.TotalMinutes));
    }

    protected override string? GetMaximumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Inverted
            ? null
            : Loc.GetString("character-timer-overall-too-high",
                ("time", playtime.TotalMinutes - Max.TotalMinutes));
    }

    protected override TimeSpan GetTotalPlaytime(CharacterRequirementContext context,
        IPrototypeManager prototypeManager)
    {
        return context.Playtimes != null
            ? context.Playtimes.GetValueOrDefault(PlayTimeTrackingShared.TrackerOverall)
            : TimeSpan.Zero;
    }
}

/// <summary>
///     Requires the playtime for a tracker to be within a certain range
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterPlaytimeRequirement : CharacterTimeRequirement
{
    [DataField(required: true)]
    public ProtoId<PlayTimeTrackerPrototype> Tracker;

    protected override TimeSpan GetTotalPlaytime(CharacterRequirementContext context,
        IPrototypeManager prototypeManager)
    {
        return context.Playtimes != null
            ? context.Playtimes.GetValueOrDefault(Tracker)
            : TimeSpan.Zero;
    }

    protected override string? GetMinimumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var jobSystem = entityManager.System<SharedJobSystem>();
        var jobStr = GetTrackerName(jobSystem, prototypeManager);
        var department = GetDepartment(jobSystem);

        return Inverted
            ? null
            : Loc.GetString("character-timer-role-insufficient",
                ("time", Min.TotalMinutes - playtime.TotalMinutes),
                ("job", jobStr),
                ("departmentColor", department?.Color ?? Color.White));
    }

    protected override string? GetMaximumText(TimeSpan playtime,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var jobSystem = entityManager.System<SharedJobSystem>();
        var jobStr = GetTrackerName(jobSystem, prototypeManager);
        var department = GetDepartment(jobSystem);

        return Inverted
            ? null
            : Loc.GetString("character-timer-role-too-high",
                ("time", playtime.TotalMinutes - Max.TotalMinutes),
                ("job", jobStr),
                ("departmentColor", department?.Color ?? Color.White));
    }

    private string GetTrackerName(SharedJobSystem jobSystem,
        IPrototypeManager prototypeManager)
    {
        var trackerJob = jobSystem.GetJobPrototype(Tracker);
        var job = prototypeManager.Index<JobPrototype>(trackerJob);
        return job.LocalizedName;
    }

    private DepartmentPrototype? GetDepartment(SharedJobSystem jobSystem)
    {
        var trackerJob = jobSystem.GetJobPrototype(Tracker);
        if (!jobSystem.TryGetPrimaryDepartment(trackerJob, out var department) &&
            !jobSystem.TryGetDepartment(trackerJob, out department))
            return null;

        return department;
    }
}
