// SPDX-FileCopyrightText: 2024 Pierson Arnold <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Consent;
using Content.Server.Mind;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly IServerConsentManager _consent = default!;
    [Dependency] private readonly MindSystem _serverMindSystem = default!;

    protected override FormattedMessage GetConsentText(NetUserId userId)
    {
        var text = _consent.GetPlayerConsentSettings(userId).Freetext;
        if (text == string.Empty)
        {
            text = Loc.GetString("consent-examine-not-set");
        }

        var message = new FormattedMessage();
        message.AddText(text);
        return message;
    }

    public bool HasConsent(Entity<MindContainerComponent?> ent, ProtoId<ConsentTogglePrototype> consentId)
    {
        if (!Resolve(ent, ref ent.Comp)
            || _serverMindSystem.GetMind(ent, ent) is not { } mind)
        {
            return true; // NPCs as well as player characters without a mind consent to everything
        }

        if (!TryComp<MindComponent>(mind, out var mindComponent)
            || mindComponent.UserId is not { } userId)
        {
            // Not sure if this is ever reached? MindComponent seems to always have UserId.
            Log.Warning("HasConsent No UserId or missing MindComponent");
            return false; // For entities that have a mind but with no user attached, consent to nothing.
        }

        return _consent.GetPlayerConsentSettings(userId).Toggles.TryGetValue(consentId, out var val) && val == "on";
    }
}
