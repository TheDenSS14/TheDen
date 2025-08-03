// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using System.Numerics;
using Content.Server.Administration;
using Content.Server.Administration.Systems;
using Content.Shared._DEN.Species;
using Content.Shared.Administration;
using Content.Shared.HeightAdjust;
using Content.Shared.Humanoid;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
sealed class SetHeightWidth : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public string Command => "setheightwidth";
    public string Description => "Set height and width with an option to ignore species restrictions.";
    public string Help => "setheightwidth <player> <height (integer or string to ignore)> <width (integer or string to ignore)> <exempt>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 3)
        {
            shell.WriteLine("Usage: setheightwidth <player> <height (integer or string to ignore)> <width (integer or string to ignore)> <exempt>");
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

        var heightInput = args[1];
        var widthInput = args[2];

        var height = 0f;
        var width = 0f;

        if (!_entManager.TryGetComponent<HumanoidAppearanceComponent>(playerEntity, out var appearance))
        {
            shell.WriteError("Player appearance not found: " + args[0]);
            return;
        }

        if (!float.TryParse(heightInput, out height))
            height = appearance.Height;

        if (!float.TryParse(widthInput, out width))
            width = appearance.Width;

        if (args.Length > 3 && bool.TryParse(args[3], out var result) && result)
            _entManager.EnsureComponent<SpeciesRestrictionExemptComponent>(playerEntity);

        if (Math.Abs(height - appearance.Height) <= 0.0002f && Math.Abs(width - appearance.Width) <= 0.0002f)
            return;

        var heightAdjust = _entManager.System<HeightAdjustSystem>();
        var rejuvenate = _entManager.System<RejuvenateSystem>();
        heightAdjust.SetScale(playerEntity, new Vector2(width, height));
        rejuvenate.PerformRejuvenate(playerEntity);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _playerManager.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-ban-hint"));
        }

        if (args.Length == 2 || args.Length == 3)
        {
            var num = args.Length == 2 ? args[1] : args[2];
            var validNumber = float.TryParse(num, out var number);
            var hint = validNumber ? number.ToString("0.00") : "<invalid number>";

            return CompletionResult.FromHint(hint);
        }

        if (args.Length == 4)
        {
            var options = new[] { "true", "false" };
            return CompletionResult.FromHintOptions(options, "<bool>");
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Admin)]
sealed class GetHeightWidth : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public string Command => "getheightwidth";
    public string Description => "Set height and width with an option to ignore species restrictions.";
    public string Help => "getheightwidth <player>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteLine("Usage: getheightwidth <player>");
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

        if (!_entManager.TryGetComponent<HumanoidAppearanceComponent>(playerEntity, out var appearance))
        {
            shell.WriteError("Player appearance not found: " + args[0]);
            return;
        }

        shell.WriteLine("Height: " + appearance.Height.ToString("0.00"));
        shell.WriteLine("Width: " + appearance.Width.ToString("0.00"));
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
