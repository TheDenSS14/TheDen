// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.CCVar;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Content.Shared.Customization.Systems._DEN;

namespace Content.Shared.Customization.Systems;


/// <summary>
///     Requires the player to be whitelisted if whitelists are enabled
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterWhitelistRequirement : CharacterRequirement
{
    // Always true, because this requirement may auto-pass if whitelists are disabled.
    public override bool PreCheckMandatory(CharacterRequirementContext context) => true;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        if (!configManager.IsCVarRegistered("whitelist.enabled")
            || !configManager.GetCVar(CCVars.WhitelistEnabled))
            return null;

        return Loc.GetString("character-whitelist-requirement", ("inverted", Inverted));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return !configManager.IsCVarRegistered("whitelist.enabled")
            || !configManager.GetCVar(CCVars.WhitelistEnabled)
            || context.Whitelisted == true;
    }
}
