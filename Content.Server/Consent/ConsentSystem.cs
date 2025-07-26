// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Consent;
using Content.Server.Mind;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly IServerConsentManager _consent = default!;
    [Dependency] private readonly MindSystem _serverMindSystem = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnPlayerAttached(PlayerAttachedEvent msg, EntitySessionEventArgs args)
    {
        var consentComponent = EnsureComp<ConsentComponent>(msg.Entity);
        var consentSettings = _consent.GetPlayerConsentSettings(args.SenderSession.UserId);

        foreach (var (protoId, consentSetting) in consentSettings.Toggles)
        {
            if (consentSetting)
                consentComponent.Consents.Add(protoId);
        }

        Dirty(msg.Entity, consentComponent);
    }

    private void OnPlayerDetached(PlayerDetachedEvent msg, EntitySessionEventArgs args)
    {
        if (!HasComp<ConsentComponent>(msg.Entity))
            return;

        RemComp<ConsentComponent>(msg.Entity);
    }

    protected override FormattedMessage GetConsentText(NetUserId userId)
    {
        var consent = _consent.GetPlayerConsentSettings(userId);
        var text = consent.Freetext;

        if (text == string.Empty)
            text = Loc.GetString("consent-examine-not-set");

        var message = new FormattedMessage();
        message.AddText(text);

        return message;
    }

    public override bool HasConsent(EntityUid uid, ProtoId<ConsentTogglePrototype> consentId)
    {
        if (!_playerManager.TryGetSessionByEntity(uid, out var playerSession))
            return false; // NPCs as well as player characters without a mind consent to nothing

        var consent = _consent.GetPlayerConsentSettings(playerSession.UserId);
        var valueFound = consent.Toggles.TryGetValue(consentId, out var consented);
        return valueFound && consented;
    }
}
