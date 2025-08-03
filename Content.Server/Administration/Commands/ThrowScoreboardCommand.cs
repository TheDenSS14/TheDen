// SPDX-FileCopyrightText: 2022 KIBORG04 <bossmira4@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.VarEdit)]
public sealed class ThrowScoreboardCommand : IConsoleCommand
{
    public string Command => "throwscoreboard";

    public string Description => Loc.GetString("throw-scoreboard-command-description");

    public string Help => Loc.GetString("throw-scoreboard-command-help-text");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length > 0)
        {
            shell.WriteLine(Help);
            return;
        }
        EntitySystem.Get<GameTicker>().ShowRoundEndScoreboard();
    }
}
