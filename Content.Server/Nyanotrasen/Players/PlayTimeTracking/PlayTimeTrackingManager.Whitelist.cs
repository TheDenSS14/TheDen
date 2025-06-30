// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Players;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Player;

namespace Content.Server.Players.PlayTimeTracking;

public sealed partial class PlayTimeTrackingManager
{
    private void SendWhitelistCached(ICommonSession playerSession)
    {
        var whitelist = playerSession.ContentData()?.Whitelisted ?? false;

        var msg = new MsgWhitelist
        {
            Whitelisted = whitelist
        };

        _net.ServerSendMessage(msg, playerSession.Channel);
    }

    /// <summary>
    /// Queue sending whitelist status to the client.
    /// </summary>
    public void QueueSendWhitelist(ICommonSession player)
    {
        if (DirtyPlayer(player) is { } data)
            data.NeedRefreshWhitelist = true;
    }
}
