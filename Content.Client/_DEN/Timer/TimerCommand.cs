// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Atmos.EntitySystems;
using Content.Shared._DEN.Timestring;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Content.Shared.GameTicking;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Client._DEN.Timer;

[AnyCommand]
internal sealed class TimerCommand : LocalizedCommands
{
    [Dependency] private readonly IEntitySystemManager _sysManager = default!;

    public override string Command => "timer";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            var argsError = LocalizationManager.GetString($"cmd-{Command}-args-error", ("command", Command));
            shell.WriteError(argsError);
            return;
        }

        var minutes = TimestringUtilities.CountMinutes(args[0]);
        var timerSystem = _sysManager.GetEntitySystem<TimerSystem>();
        var gameTicker = _sysManager.GetEntitySystem<SharedGameTicker>();
        var curTime = gameTicker.RoundDuration();
        var runAt = curTime + TimeSpan.FromMinutes(minutes);
        var success = LocalizationManager.GetString(
            $"cmd-{Command}-success",
                ("command", Command),
                ("run-at", runAt.ToString(@"hh\:mm\:ss"))
            );

        timerSystem.AddSimpleTimer(runAt);
        shell.WriteLine(success);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHint("<timestring (e.g. 4h23m)>");

        return CompletionResult.Empty;
    }
}
