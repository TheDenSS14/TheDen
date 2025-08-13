// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Mind;
using Content.Shared._Floof.Consent;
using Content.Shared.GameTicking;
using Robust.Shared.Network;
using Robust.Shared.Utility;


namespace Content.Server._Floof.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly ConsentSystem _consent = default!;

    protected override FormattedMessage GetConsentText(NetUserId userId)
    {
        TryGetConsent(userId, out var consent);
        var text = consent?.Freetext ?? string.Empty;

        if (text == string.Empty)
            text = Loc.GetString("consent-examine-not-set");

        var message = new FormattedMessage();
        message.AddText(text);
        return message;
    }

    public override void SetConsent(NetUserId userId, PlayerConsentSettings? consentSettings)
    {
        base.SetConsent(userId, consentSettings);

        if (consentSettings == null)
        {
            UserConsents.Remove(userId);
            return;
        }

        UserConsents[userId] = consentSettings;
    }
}
