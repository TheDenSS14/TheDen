// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.IdentityManagement.Components;
using Content.Shared.Mind.Components;
using Content.Shared.Paper;
using Content.Shared.Roles;


namespace Content.Server._DEN.Paper;


public sealed class SignTagSystem : EntitySystem
{
    [Dependency] private readonly PaperSystem _paperSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PaperComponent, PaperComponent.PaperSignatureRequestMessage>(OnSignatureRequested);
    }

    private void OnSignatureRequested(Entity<PaperComponent> ent, ref PaperComponent.PaperSignatureRequestMessage args)
    {
        Log.Info("signature requested");
        var signature = GetPlayerSignature(args.Actor);
        var content = ReplaceNthSignatureTag(ent.Comp.Content, args.SignatureIndex, signature);
        _paperSystem.SetContent(ent, content);
    }

    /// <summary>
    /// Gets the player's signature using the identity system, including rank, name, and role.
    /// </summary>
    private string GetPlayerSignature(EntityUid player)
    {
        var name = string.Empty;
        var rank = string.Empty;
        var role = string.Empty;

        // Get the identity entity (ID card, etc.)
        var identityEntity = player;
        if (TryComp<IdentityComponent>(player, out var identity) &&
            identity.IdentityEntitySlot.ContainedEntity is { } idEntity)
        {
            identityEntity = idEntity;
        }

        // Get name from identity or fallback to entity name
        name = MetaData(identityEntity).EntityName;

        // Get role from mind system
        if (TryComp<MindContainerComponent>(player, out var mindContainer) &&
            mindContainer.Mind != null)
        {
            var roleSystem = EntityManager.System<SharedRoleSystem>();
            var roleInfo = roleSystem.MindGetAllRoleInfo((mindContainer.Mind.Value, null));
            if (roleInfo.Count > 0)
            {
                role = Loc.GetString(roleInfo[0].Name);
            }
        }

        // Format: "Rank Name, Role" or fallback combinations
        string signature;

        if (!string.IsNullOrEmpty(rank) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(role))
        {
            signature = $"{rank} {name}, {role}";
        }
        else if (!string.IsNullOrEmpty(rank) && !string.IsNullOrEmpty(name))
        {
            signature = $"{rank} {name}";
        }
        else if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(role))
        {
            signature = $"{name}, {role}";
        }
        else
        {
            signature = name;
        }

        return signature;
    }

    /// <summary>
    /// Replaces the nth occurrence of [signature] tag with replacement text.
    /// </summary>
    private static string ReplaceNthSignatureTag(string text, int index, string replacement)
    {
        const string signatureTag = "[signature]";
        var currentIndex = 0;
        var pos = 0;

        while (pos < text.Length)
        {
            var foundPos = text.IndexOf(signatureTag, pos);
            if (foundPos == -1)
                break;

            if (currentIndex == index)
            {
                return text.Substring(0, foundPos) + replacement + text.Substring(foundPos + signatureTag.Length);
            }

            currentIndex++;
            pos = foundPos + signatureTag.Length;
        }

        return text;
    }
}
