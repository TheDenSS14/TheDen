// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Administration;
using Content.Server.Announcements.Systems;
using Content.Shared.Administration;
using Content.Shared.Announcements.Prototypes;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Round)]
public sealed class ChangeAnnouncerCommand : IConsoleCommand
{
    [Dependency] private readonly IEntitySystemManager _entitySystem = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    private const int MinimumArgumentCount = 1;
    private const int MaximumArgumentCount = 3;

    public string Command => "setannouncer";
    public string Description => Loc.GetString("cmd-set-announcer-desc");
    public string Help => Loc.GetString("cmd-set-announcer-help", ("command", Command));

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        // Too many / not enough arguments
        if (args.Length < MinimumArgumentCount || args.Length > MaximumArgumentCount)
        {
            var invalidArgCountMessage = Loc.GetString("cmd-set-announcer-error-arg-count",
                ("min", MinimumArgumentCount),
                ("max", MaximumArgumentCount));
            shell.WriteError(invalidArgCountMessage);
            shell.WriteLine(Help);
            return;
        }

        var announcerId = args[0];

        // Incorrect announcer ID
        if (!_prototype.TryIndex<AnnouncerPrototype>(announcerId, out var announcer))
        {
            shell.WriteError(Loc.GetString("cmd-set-announcer-error-invalid-announcer-id"));

            var announcers = string.Join(", ", GetAnnouncers());
            shell.WriteLine(Loc.GetString("cmd-set-announcer-error-list-announcers", ("announcers", announcers)));

            return;
        }

        var announcerSystem = _entitySystem.GetEntitySystem<AnnouncerSystem>();

        // Tried to set the same announcer
        if (announcerSystem.Announcer.ID == announcerId)
        {
            shell.WriteError(Loc.GetString("cmd-set-announcer-error-same-announcer", ("announcer", announcerId)));
            return;
        }

        var announceChange = true;
        var changeArgExists = args.TryGetValue(1, out var announceChangeArg);

        // Incorrect "should announce" argument
        if (changeArgExists)
        {
            if (bool.TryParse(announceChangeArg, out var shouldChange))
                announceChange = shouldChange;
            else
            {
                // Tried to provide an "announceChange" argument, but it was invalid.
                // Even though this is an optional value, if you provide an invalid value, we do not
                // carry out the command anyway, so that we don't cause any undesirable behavior.
                shell.WriteError(Loc.GetString("cmd-set-announcer-error-invalid-announce-change"));
                shell.WriteLine(Help);
                return;
            }
        }

        args.TryGetValue(2, out var changeReason);

        // Invalid reason (how???)
        if (changeReason != null && changeReason.Trim() == String.Empty)
        {
            shell.WriteError(Loc.GetString("cmd-set-announcer-error-invalid-reason"));
            shell.WriteLine(Help);
        }

        // Everything's good! Let's roll!
        announcerSystem.SetAnnouncer(announcer);

        if (announceChange)
            announcerSystem.AnnounceChangedAnnouncer(changeReason);

        shell.WriteLine(Loc.GetString("cmd-set-announcer-success", ("announcer", announcerId)));
    }

    private List<string> GetAnnouncers()
    {
        var announcers = new List<string>();

        foreach (var announcer in _prototype.EnumeratePrototypes<AnnouncerPrototype>())
            announcers.Add(announcer.ID);

        return announcers;
    }
}
