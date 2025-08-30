// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Administration.Managers;
using Content.Shared.Chat;
using Content.Shared.Customization.Systems;
using Content.Shared.Customization.Systems._DEN;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DEN.Customization.CharacterRequirements;

/// <summary>
///     Requires the player to be an admin, regardless of permissions.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterAdminRequirement : CharacterRequirement
{
    [DataField]
    public bool IncludeDeadminned = true;

    private static string RequirementColor => ChatChannel.AdminChat.TextColor().ToHex();

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Player is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        return Loc.GetString("character-requirement-admin",
            ("inverted", Inverted),
            ("color", RequirementColor));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var adminManager = IoCManager.Resolve<ISharedAdminManager>();
        return context.Player != null
            && adminManager.IsAdmin(context.Player, IncludeDeadminned);
    }
}

/// <summary>
///     Requires the player to have the given admin flags.
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class CharacterAdminFlagRequirement : CharacterRequirement
{
    [DataField]
    public bool IncludeDeadminned = true;

    [DataField]
    public AdminFlags Flags = AdminFlags.Admin;

    private static string RequirementColor => ChatChannel.AdminChat.TextColor().ToHex();

    public override bool PreCheckMandatory(CharacterRequirementContext context)
        => context.Player is not null;

    public override string? GetReason(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var flagNames = GetFlagNames().Select(flag => $"[color={RequirementColor}]{flag}[/color]");
        var flagNameList = string.Join(", ", flagNames);
        return Loc.GetString("character-requirement-admin-flags",
            ("inverted", Inverted),
            ("flags", flagNameList));
    }

    public override bool IsValid(CharacterRequirementContext context,
        IEntityManager entityManager,
        IPrototypeManager prototypeManager,
        IConfigurationManager configManager)
    {
        var adminManager = IoCManager.Resolve<ISharedAdminManager>();
        return context.Player != null
            && adminManager.HasAdminFlag(context.Player, Flags, IncludeDeadminned);
    }

    private IEnumerable<string> GetFlagNames()
    {
        foreach (var value in Enum.GetValues<AdminFlags>())
        {
            if ((int) Flags > 0 && (int) value == 0)
                continue;

            if (Flags.HasFlag(value))
                yield return value.ToString();
        }
    }
}
