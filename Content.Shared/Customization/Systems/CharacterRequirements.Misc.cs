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

/// <summary>
///     Requires the server to have a specific CVar value.
/// </summary>
[UsedImplicitly, Serializable, NetSerializable,]
public sealed partial class CVarRequirement : CharacterRequirement
{
    [DataField("cvar", required: true)]
    public string CVar;

    [DataField(required: true)]
    public string RequiredValue;

    private const string RequirementColor = "lightblue";

    // Always true because context is unused.
    public override bool PreCheckMandatory(CharacterRequirementContext context) => true;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (!configManager.IsCVarRegistered(CVar))
            return null;

        return Loc.GetString(
            "character-cvar-requirement",
            ("inverted", Inverted),
            ("color", RequirementColor),
            ("cvar", CVar),
            ("value", RequiredValue));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (!configManager.IsCVarRegistered(CVar))
            return true;

        var cvar = configManager.GetCVar(CVar);
        var valid = cvar.ToString()! == RequiredValue;
        return valid;
    }
}
