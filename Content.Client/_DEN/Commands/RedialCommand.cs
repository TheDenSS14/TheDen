// SPDX-FileCopyrightText: 2025 Cami
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Client;
using Robust.Shared.Console;

namespace Content.Client._DEN.Commands;

[UsedImplicitly, AnyCommand]
public sealed class RedialCommand : IConsoleCommand
{
    [Dependency] private readonly IGameController _gameController = default!;

    public string Command => "redial";
    public string Description => "Connects to a server via the specified server address.";
    public string Help => "Usage: redial <address>\nExample: redial ss14://0.0.0.0:1215";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteLine(Help);
            return;
        }

        try
        {
            _gameController.Redial(args[0]);
            shell.WriteLine($"Initiating redial to {args[0]}");
        }
        catch (Exception ex)
        {
            shell.WriteError($"Redial failed: {ex.Message}");
        }
    }
}
