// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.Ghost;
using Robust.Server.Player;
using Robust.Shared.Player;
using Robust.Shared.Utility;


namespace Content.Server._DEN.Redial;


/// <summary>
/// This handles entity-based redial requests.
/// </summary>
public sealed class RedialSystem : EntitySystem
{
    [Dependency] private readonly IChatManager _chatManager = null!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly RedialManager _redialManager = null!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerJoinedLobbyEvent>(OnJoinLobby);
    }

    public void SetSavedRedial(string redialAddress, string redialMessage, bool notifyOrPopup = true)
    {
        _redialManager.SetSavedRedial(redialAddress, redialMessage);

        if (notifyOrPopup)
        {
            SendRedialNotificationAndPopup();
        }
    }

    public void SendRedialNotificationAndPopup()
    {
        var redialMessage = _redialManager.GetRedialMessage();
        var announcement = Loc.GetString("redial-chat-notification", ("message", redialMessage));
        _chatManager.DispatchServerAnnouncement(announcement);

        foreach (var session in _playerManager.Sessions)
        {
            // ReSharper disable once ArrangeTrailingCommaInSinglelineLists
            if (session.AttachedEntity is { Valid: true } attachedEntity
                && !HasComp<GhostComponent>(attachedEntity))
                continue;

            HandleLobby(session);
        }
    }

    private void HandleLobby(ICommonSession session) => _redialManager.SendSavedRedialPopup(session.Channel);

    private void OnJoinLobby(PlayerJoinedLobbyEvent ev) =>
        _redialManager.SendSavedRedialPopup(ev.PlayerSession.Channel);
}
