// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Customization.Systems._DEN;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Customization.Systems;

// ReSharper disable InvalidXmlDocComment
[ImplicitDataDefinitionForInheritors, MeansImplicitUse]
[Serializable, NetSerializable]
public abstract partial class CharacterRequirement
{
    /// <summary>
    ///     If true valid requirements will be treated as invalid and vice versa
    ///     This inversion is done by other systems like <see cref="CharacterRequirementsSystem"/>, not this one
    /// </summary>
    [DataField]
    public bool Inverted = false;

    /// <summary>
    ///     When a character requirement fails PreCheckMandatory(), it will count as a valid requirement check,
    ///     assuming that the context is simply ignoring this requirement. If the requirement is mandatory, then
    ///     the check will be considered invalid if it fails pre-checking.
    /// </summary>
    /// <remarks>
    ///     For example: if you're checking a job requirement that requires SelectedJob, and context.SelectedJob is
    ///     null, we will assume that it auto-passes because the job is irrelevant. If this requrement is mandatory,
    ///     it auto-fails in this case instead, assuming the job is required.
    /// </remarks>
    [DataField]
    public bool Mandatory = false;

    /// <summary>
    ///     Check if the given context has all the information it needs to perform a mandatory check.
    /// </summary>
    /// <param name="context">The contextual information about the entity being checked.</param>
    public abstract bool PreCheckMandatory(CharacterRequirementContext context);

    /// <summary>
    ///     Checks if this character requirement is valid for the given parameters
    ///     <br />
    ///     You should probably not be calling this directly, use <see cref="CharacterRequirementsSystem"/>
    /// </summary>
    /// <param name="reason">Description for the requirement, shown when not null</param>
    public abstract bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager);

    public abstract string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager);
}
