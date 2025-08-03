// SPDX-FileCopyrightText: 2024 Pierson Arnold
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Shared.Administration.Logs;
using Content.Shared.Consent;
using Content.Shared.Database;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Consent;

public sealed class ServerConsentManager : IServerConsentManager
{
    [Dependency] private readonly IConfigurationManager _configManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IServerNetManager _netManager = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

    private ConsentSystem? _consentSystem;

    public void Initialize()
    {
        _consentSystem = _entityManager.System<ConsentSystem>();
        _netManager.RegisterNetMessage<MsgUpdateConsent>(HandleUpdateConsentMessage);
    }

    private async void HandleUpdateConsentMessage(MsgUpdateConsent message)
    {
        var userId = message.MsgChannel.UserId;

        if (!_consentSystem!.TryGetConsent(userId, out _))
            return;

        message.Consent.EnsureValid(_configManager, _prototypeManager);
        _consentSystem.SetConsent(userId, message.Consent);

        var session = _playerManager.GetSessionByChannel(message.MsgChannel);
        var togglesPretty = String.Join(", ", message.Consent.Toggles.Select(t => $"[{t.Key}: {t.Value}]"));
        _adminLogger.Add(LogType.Consent, LogImpact.Medium,
            $"{session:Player} updated consent setting to: '{message.Consent.Freetext}' with toggles {togglesPretty}");

        if (ShouldStoreInDb(message.MsgChannel.AuthType))
            await _db.SavePlayerConsentSettingsAsync(userId, message.Consent);

        // send it back to confirm to client that consent was updated
        _netManager.ServerSendMessage(message, message.MsgChannel);
    }

    public async Task LoadData(ICommonSession session, CancellationToken cancel)
    {
        var consent = new PlayerConsentSettings();

        if (ShouldStoreInDb(session.AuthType))
            consent = await _db.GetPlayerConsentSettingsAsync(session.UserId);

        consent.EnsureValid(_configManager, _prototypeManager);
        _consentSystem!.SetConsent(session.UserId, consent);

        var message = new MsgUpdateConsent() { Consent = consent };
        _netManager.ServerSendMessage(message, session.Channel);
    }

    public void OnClientDisconnected(ICommonSession session)
    {
        _consentSystem!.SetConsent(session.UserId, null);
    }

    /// <inheritdoc />
    public PlayerConsentSettings GetPlayerConsentSettings(NetUserId userId)
    {
        if (_consentSystem!.TryGetConsent(userId, out var consent))
            return consent;

        // A player that has disconnected does not consent to anything.
        return new();
    }

    private static bool ShouldStoreInDb(LoginType loginType)
    {
        return loginType.HasStaticUserId();
    }
}
