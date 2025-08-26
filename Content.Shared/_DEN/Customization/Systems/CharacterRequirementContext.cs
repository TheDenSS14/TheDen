// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

#pragma warning disable IDE1006
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared.Customization.Systems._DEN;
#pragma warning restore IDE1006

/// <summary>
/// A class that stores contextual information about a player (or other thing) that needs to check its properties
/// against CharacterRequirements - such as playtime, whitelist, current job, etc.
/// </summary>
/// <remarks>
/// Fields of this class are nullable to indicate optional parameters. If there is a particular requirement
/// that does not need checking, then leave the field nullable. Keep in mind that CharacterRequirements may have
/// a "mandatory" field that will not ignore null fields and simply assume null fields fail requirements.
/// </remarks>.
public sealed partial class CharacterRequirementContext
{
    /// <summary>
    /// The currently selected job of this entity.
    /// </summary>
    public JobPrototype? SelectedJob = null;

    /// <summary>
    /// The currently loaded character profile of this entity.
    /// </summary>
    public HumanoidCharacterProfile? Profile = null;

    /// <summary>
    /// A map of PlaytimeTracker IDs to playtime durations.
    /// </summary>
    public Dictionary<string, TimeSpan>? Playtimes = null;

    /// <summary>
    /// Whether or not this entity is whitelisted.
    /// </summary>
    public bool? Whitelisted = null;

    /// <summary>
    /// The current prototype that this entity is representing.
    /// </summary>
    public IPrototype? Prototype = null;

    /// <summary>
    /// The entity that is representing this context.
    /// </summary>
    public EntityUid? Entity = null;

    /// <summary>
    /// Used for logical requirements - this represents how many logical requirements deep we are.
    /// </summary>
    public int? Depth = null;

    public CharacterRequirementContext(
        JobPrototype? selectedJob = null,
        HumanoidCharacterProfile? profile = null,
        Dictionary<string, TimeSpan>? playtimes = null,
        bool? whitelisted = null,
        IPrototype? prototype = null,
        EntityUid? entity = null,
        int? depth = null)
    {
        SelectedJob = selectedJob;
        Profile = profile;
        Playtimes = playtimes;
        Whitelisted = whitelisted;
        Prototype = prototype;
        Entity = entity;
        Depth = depth;
    }

    public CharacterRequirementContext(CharacterRequirementContext other)
    {
        SelectedJob = other.SelectedJob;
        Profile = other.Profile;
        Playtimes = other.Playtimes != null ? new Dictionary<string, TimeSpan>(other.Playtimes) : null;
        Whitelisted = other.Whitelisted;
        Prototype = other.Prototype;
        Entity = other.Entity;
        Depth = other.Depth;
    }

    public CharacterRequirementContext ShallowClone() => new(this);

    public CharacterRequirementContext WithSelectedJob(JobPrototype? selectedJob)
        => new(this) { SelectedJob = selectedJob };

    public CharacterRequirementContext WithProfile(HumanoidCharacterProfile? profile)
        => new(this) { Profile = profile };

    public CharacterRequirementContext WithPlaytimes(Dictionary<string, TimeSpan>? playtimes)
        => new(this) { Playtimes = playtimes };

    public CharacterRequirementContext WithWhitelisted(bool? whitelisted) => new(this) { Whitelisted = whitelisted };

    public CharacterRequirementContext WithPrototype(IPrototype? prototype) => new(this) { Prototype = prototype };

    public CharacterRequirementContext WithEntity(EntityUid? entity) => new(this) { Entity = entity };

    public CharacterRequirementContext WithDepth(int? depth) => new(this) { Depth = depth };
}
