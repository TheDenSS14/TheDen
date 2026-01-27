// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using System.Numerics;
using Content.Server._Floof.Consent;
using Content.Server.Administration;
using Content.Server.Administration.Systems;
using Content.Shared._DEN.Species;
using Content.Shared._Floof.Consent;
using Content.Shared.Administration;
using Content.Shared.HeightAdjust;
using Content.Shared.Humanoid;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
sealed class HasConsent : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entityManager = null!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = null!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;

    public string Command => "hasconsent";
    public string Description => "Check if a player has a consent enabled.";
    public string Help => "hasconsent <player> <consent>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 2)
        {
            shell.WriteLine("Usage: hasconsent <player> <consent>");
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

        if (!_prototypeManager.TryIndex<ConsentTogglePrototype>(args[1], out var consentToggle))
        {
            shell.WriteError("Unknown consent: " + args[1]);
            return;
        }

        var consentSystem = _entityManager.System<ConsentSystem>();
        var consent = consentSystem.HasConsent(playerEntity, consentToggle);
        var hasConsent = consent ? "has" : "doesn't have";

        shell.WriteLine($"Player {_entityManager.ToPrettyString(playerEntity)} {hasConsent} this consent ({args[1]}) on.");
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _playerManager.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-ban-hint"));
        }

        if (args.Length == 2)
        {
            var options = CompletionHelper.PrototypeIDs<ConsentTogglePrototype>();
            return CompletionResult.FromOptions(options);
        }

        return CompletionResult.Empty;
    }
}
