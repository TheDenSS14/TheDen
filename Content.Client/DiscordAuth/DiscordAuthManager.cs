// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DiscordAuth;
using Robust.Client.State;
using Robust.Shared.Network;

namespace Content.Client.DiscordAuth;

public sealed class DiscordAuthManager
{
    [Dependency] private readonly IClientNetManager _net = default!;
    [Dependency] private readonly IStateManager _state = default!;


    public string AuthUrl { get; private set; } = string.Empty;


    public void Initialize()
    {
        _net.RegisterNetMessage<DiscordAuthCheckMessage>();
        _net.RegisterNetMessage<DiscordAuthRequiredMessage>(OnDiscordAuthRequired);
    }


    private void OnDiscordAuthRequired(DiscordAuthRequiredMessage message)
    {
        if (_state.CurrentState is not DiscordAuthState)
        {
            AuthUrl = message.AuthUrl;
            _state.RequestStateChange<DiscordAuthState>();
        }
    }
}
