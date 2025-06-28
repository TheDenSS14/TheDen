// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DEN.Voting.Systems;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;


namespace Content.Server._DEN.Administration.Commands;


[AdminCommand(AdminFlags.Admin)]
sealed class SetHighDangerCommand : IConsoleCommand
{
    public string Command => "sethighdanger";
    public string Description => "Sets the current round to being that of a high danger nature.";
    public string Help => "sethighdanger";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var sysManager = IoCManager.Resolve<IEntitySystemManager>();
        var duplicateVote = sysManager.GetEntitySystem<DuplicateVoteSystem>();

        duplicateVote.IsHighDanger = true;
    }
}
