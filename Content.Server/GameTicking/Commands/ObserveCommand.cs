// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Content.Shared.GameTicking;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.Commands
{
    [AnyCommand]
    sealed class ObserveCommand : IConsoleCommand
    {
        public string Command => "observe";
        public string Description => "";
        public string Help => "";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                return;
            }

            var ticker = EntitySystem.Get<GameTicker>();

            if (ticker.RunLevel == GameRunLevel.PreRoundLobby)
            {
                shell.WriteError("Wait until the round starts.");
                return;
            }

            if (ticker.PlayerGameStatuses.TryGetValue(player.UserId, out var status) &&
                status != PlayerGameStatus.JoinedGame)
            {
                ticker.JoinAsObserver(player);
            }
            else
            {
                shell.WriteError($"{player.Name} is not in the lobby.   This incident will be reported.");
            }
        }
    }
}
