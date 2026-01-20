// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.ServerContent;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Customization.CharacterRequirements;

/// <summary>
///     Requires the server to have the given content ID.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterServerSelectiveRequirement : CharacterRequirement
{
    [DataField]
    public string ServerId = string.Empty;

    private static string _requirementColor = Color.White.ToHex();

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => true;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var serverName = ServerSelectiveContentManager.GetServerName(ServerId);

        return Loc.GetString("character-requirement-server-selective",
            ("inverted", Inverted),
            ("color", _requirementColor),
            ("serverName", serverName));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var serverSelective = IoCManager.Resolve<ServerSelectiveContentManager>();
        return serverSelective.IsServer(ServerId);
    }
}
