// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server._DV.ParadoxAnomaly.Systems;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;


namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
sealed class CreateParadoxAnomalyCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public string Command => "createparadoxanomaly";
    public string Description => "Create a paradox anomaly of a player. Follows consent.";
    public string Help => "createparadoxanomaly <player>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteLine("Usage: createparadoxanomaly <player>");
            return;
        }

        if (!_playerManager.TryGetSessionByUsername(args[0], out var session))
        {
            shell.WriteError("Session not found: " + args[0]);
            return;
        }

        if (session.AttachedEntity is not { Valid: true } playerEntity)
        {
            shell.WriteError("Player entity not found: " + args[0]);
            return;
        }

        var paradoxAnomalySystem = _entManager.System<ParadoxAnomalySystem>();
        var hasSpawned = paradoxAnomalySystem.TrySpawnUserParadoxAnomaly(playerEntity, out _);

        if (hasSpawned)
            shell.WriteLine("Success!");
        else
            shell.WriteError("Failed to spawn paradox anomaly. If they're a humanoid, is their consent set wrong?");
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _playerManager.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-ban-hint"));
        }

        return CompletionResult.Empty;
    }
}
