// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;
using Robust.Shared.Utility;

namespace Content.Server.Administration.Commands;

[UsedImplicitly]
[AdminCommand(AdminFlags.Stealth)]
public sealed class StealthminCommand : LocalizedCommands
{
    public override string Command => "stealthmin";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine(Loc.GetString("cmd-stealthmin-no-console"));
                return;
            }

            var mgr = IoCManager.Resolve<IAdminManager>();
            var adminData = mgr.GetAdminData(player);

            DebugTools.AssertNotNull(adminData);

            if (!adminData!.Stealth)
                mgr.Stealth(player);
            else
                mgr.UnStealth(player);
    }
}
