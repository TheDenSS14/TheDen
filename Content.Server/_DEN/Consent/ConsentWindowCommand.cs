// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._DEN.Consent;

[AnyCommand]
sealed class ConsentWindowCommand : IConsoleCommand
{
    [Dependency] private readonly EuiManager _eui = default!;

    public const string CommandName = "consentprefs";

    public string Command => CommandName;
    public string Description => Loc.GetString("consent-window-command-description");
    public string Help => Loc.GetString("consent-window-command-help", ("command", CommandName));

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError("This does not work from the server console.");
            return;
        }

        var consent = new ConsentEui();
        _eui.OpenEui(consent, player);
    }
}
