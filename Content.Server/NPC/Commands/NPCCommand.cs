// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.EUI;
using Content.Server.NPC.UI;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.NPC.Commands;

[AdminCommand(AdminFlags.Debug)]
public sealed class NPCCommand : IConsoleCommand
{
    public string Command => "npc";
    public string Description => "Opens the debug window for NPCs";
    public string Help => $"{Command}";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } playerSession)
        {
            return;
        }

        var euiManager = IoCManager.Resolve<EuiManager>();
        euiManager.OpenEui(new NPCEui(), playerSession);
    }
}
