// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using Content.Shared.DiscordAuth;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.Network;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Client.DiscordAuth;

public sealed class DiscordAuthState : State
{
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IClientNetManager _net = default!;


    private DiscordAuthGui? _gui;
    private readonly CancellationTokenSource _checkTimerCancel = new();


    protected override void Startup()
    {
        _gui = new DiscordAuthGui();
        _userInterface.StateRoot.AddChild(_gui);

        Timer.SpawnRepeating(TimeSpan.FromSeconds(5), () =>
        {
            _net.ClientSendMessage(new DiscordAuthCheckMessage());
        }, _checkTimerCancel.Token);
    }

    protected override void Shutdown()
    {
        _checkTimerCancel.Cancel();
        _gui!.Dispose();
    }
}
