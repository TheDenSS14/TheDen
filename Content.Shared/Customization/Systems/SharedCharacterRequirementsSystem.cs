// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 Remuchi
// SPDX-FileCopyrightText: 2025 Skubman
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using System.Text;
using Content.Shared.Customization.Systems._DEN;
using Content.Shared.Inventory;
using Content.Shared.Mind;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles.Jobs;
using Content.Shared.Station;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Customization.Systems;


public abstract partial class SharedCharacterRequirementsSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedJobSystem _jobSystem = default!;
    [Dependency] private readonly SharedMindSystem _mindSystem = default!;
    [Dependency] private readonly SharedStationSpawningSystem _stationSpawningSystem = default!;

    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPrototypeManager _protomanager = default!;
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    [Dependency] private readonly ISharedPlaytimeManager _playtimeManager = default!;

    public bool CheckRequirementValid(CharacterRequirement requirement,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        // Check if the context has the required fields for the operation.
        // If not, it auto-passes unless the requirement is mandatory, in which case it auto-fails..
        if (!requirement.PreCheckMandatory(context))
            return !requirement.Mandatory;

        var valid = requirement.IsValid(context,
            entityManager,
            prototypeManager,
            configManager);

        return valid != requirement.Inverted;
    }

    /// <summary>
    ///     Checks if a character entity meets the specified requirements.
    /// </summary>
    /// <param name="requirements">A list of requirements to check.</param>
    /// <param name="characterUid">The entity ID of the charater.</param>
    /// <param name="prototype">The prototype associated with the requirements.</param>
    /// <param name="depth">The depth of this requirement, used in logical operators.</param>
    /// <param name="whitelisted">Whether or not the player associated with the character is whitelisted.</param>
    /// <returns>True if all requirements are met, false otherwise.</returns>
    public bool CheckRequirementsValid(List<CharacterRequirement> requirements,
        EntityUid characterUid,
        IPrototype prototype,
        int depth = 0,
        bool whitelisted = false)
    {
        if (!_mindSystem.TryGetMind(characterUid, out var mindId, out var mind)
            || mind.Session == null
            || !_jobSystem.MindTryGetJob(mindId, out var jobPrototype)
            || !_stationSpawningSystem.GetProfile(characterUid, out var stationSpawningProfile)
            || !_playtimeManager.TryGetTrackerTimes(mind.Session, out var trackerTimes))
            return false;

        var context = new CharacterRequirementContext(selectedJob: jobPrototype,
            profile: stationSpawningProfile,
            playtimes: trackerTimes,
            whitelisted: whitelisted,
            prototype: prototype,
            entity: characterUid,
            depth: depth,
            player: mind.Session);

        return CheckRequirementsValid(requirements,
            context,
            _entManager,
            _protomanager,
            _configurationManager);
    }

    /// <summary>
    ///     Checks a requirements context against a list of requirements. The requirements context represents all
    ///     relevant information needed to determine if something passes the requirements or not; null fields in
    ///     the context represent irrelevant data that we do not need to check.
    /// </summary>
    /// <param name="requirements">The requirements to check against.</param>
    /// <param name="context">A class of contextual data associated with the thing being checked.</param>
    /// <returns>Whether or not the given context passes ALL requirements.</returns>
    public bool CheckRequirementsValid(List<CharacterRequirement> requirements,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        foreach (var requirement in requirements)
        {
            if (!CheckRequirementValid(requirement,
                context,
                entityManager,
                prototypeManager,
                configManager))
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Gets a list of "reasons" for a list of requirements - in other words, flavor text describing the conditions
    ///     needed in order to pass the requirement.
    /// </summary>
    /// <param name="requirements">The requirements to check against.</param>
    /// <param name="context">A class of contextual data associated with the thing being checked.</param>
    /// <returns>A list of markup strings describing requirements.</returns>
    public List<string> GetReasons(List<CharacterRequirement> requirements,
        CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var reasons = new List<string>();
        foreach (var requirement in requirements)
        {
            var reason = requirement.GetReason(context,
                entityManager,
                prototypeManager,
                configManager);

            if (reason != null)
                reasons.Add(reason);
        }

        return reasons;
    }

    /// <summary>
    ///     Gets the reason text from <see cref="CheckRequirementsValid"/> as a <see cref="FormattedMessage"/>.
    /// </summary>
    public FormattedMessage GetRequirementsText(List<string> reasons)
    {
        return FormattedMessage.FromMarkupOrThrow(GetRequirementsMarkup(reasons));
    }

    /// <summary>
    ///     Gets the reason text from <see cref="CheckRequirementsValid"/> as a markup string.
    /// </summary>
    public string GetRequirementsMarkup(List<string> reasons)
    {
        var text = new StringBuilder();
        foreach (var reason in reasons)
            text.Append($"\n{reason}");

        return text.ToString().Trim();
    }

    /// <summary>
    ///     Returns true if the given dummy can equip the given item.
    ///     Does not care if items are already in equippable slots, and ignores pockets.
    /// </summary>
    public bool CanEntityWearItem(EntityUid dummy, EntityUid clothing, bool bypassAccessCheck = false)
    {
        return _inventory.TryGetSlots(dummy, out var slots)
            && slots.Where(slot => !slot.SlotFlags.HasFlag(SlotFlags.POCKET))
                .Any(slot => _inventory.CanEquip(dummy, clothing, slot.Name, out _, onSpawn: true, bypassAccessCheck: bypassAccessCheck));
    }
}
