// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.EUI;
using Content.Server.Fax.AdminUI;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class FaxUiCommand : IConsoleCommand
{
    public string Command => "faxui";

    public string Description => Loc.GetString("cmd-faxui-desc");
    public string Help => Loc.GetString("cmd-faxui-help");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var player = shell.Player;
        if (player == null)
        {
            shell.WriteLine("shell-only-players-can-run-this-command");
            return;
        }

        var eui = IoCManager.Resolve<EuiManager>();
        var ui = new AdminFaxEui();
        eui.OpenEui(ui, player);
    }
}

