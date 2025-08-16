// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Mind;
using Content.Server.Station.Systems;
using Content.Shared._Floof.Consent;
using Content.Shared.GameTicking;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Utility;


namespace Content.Server._Floof.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly ConsentSystem _consent = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = default!;

    protected override FormattedMessage GetConsentText(NetUserId userId)
    {
        TryGetConsent(userId, out var consent);
        var text = consent?.Freetext ?? string.Empty;

        if (text == string.Empty)
            text = Loc.GetString("consent-examine-not-set");

        text += GetCharacterConsent(userId); // DEN: per-character consent.

        var message = new FormattedMessage();
        message.AddText(text);

        return message;
    }

    private string GetCharacterConsent(NetUserId userId)
    {
        var result = string.Empty;
        var hasSession = _playerManager.TryGetSessionById(userId, out var session);

        if (hasSession && session != null
            && _stationSpawning.GetProfile(session.AttachedEntity, out var profile)
            && !string.IsNullOrWhiteSpace(profile.CharacterConsent))
        {
            result += $"\n\n- [{profile.Name}] -";
            result += $"\n{profile.CharacterConsent}";
        }

        return result;
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
