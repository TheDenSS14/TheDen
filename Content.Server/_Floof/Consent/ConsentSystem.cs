// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Mind;
using Content.Server.Station.Systems;
using Content.Shared._DEN.Consent;
using Content.Shared._Floof.Consent;
using Content.Shared.GameTicking;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;


namespace Content.Server._Floof.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = null!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = null!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnSpawnComplete);
        SubscribeLocalEvent<PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnSpawnComplete(PlayerSpawnCompleteEvent msg, EntitySessionEventArgs args) =>
        RefreshConsents(args.SenderSession);

    private void OnPlayerAttached(PlayerAttachedEvent args) =>
        RefreshConsents(args.Player);

    // mindless entities should not have old consent preferences
    private void OnPlayerDetached(PlayerDetachedEvent args) =>
        RemComp<ConsentComponent>(args.Entity);

    private void RefreshConsents(ICommonSession session)
    {
        if (session.AttachedEntity is not { Valid: true } attachedEntity)
            return;

        var consentComponent = EnsureComp<ConsentComponent>(attachedEntity);

        foreach (var consent in _prototypeManager.EnumeratePrototypes<ConsentTogglePrototype>())
        {
            var exists = UserConsents[session.UserId].Toggles.TryGetValue(consent.ID, out var value);
            var consentValue = exists && value == "on";

            if (!exists || consentValue == consent.DefaultValue)
                continue;

            consentComponent.NotDefaultConsents.Add(consent.ID);
        }

        Dirty<ConsentComponent>((attachedEntity, consentComponent));
    }

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

        if (_playerManager.TryGetSessionById(userId, out var session))
            RefreshConsents(session);
    }
}
