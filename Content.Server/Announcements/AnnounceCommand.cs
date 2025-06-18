// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2021 moonheart08 <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Myctai <108953437+Myctai@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Administration;
using Content.Server.Announcements.Systems;
using Content.Shared.Administration;
using Content.Shared.Announcements.Prototypes;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Announcements
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AnnounceCommand : IConsoleCommand
    {
        public string Command => "announce";
        public string Description => "Send an in-game announcement.";
        public string Help => $"{Command} <sender> <message> <sound> <announcer>";
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var announcer = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<AnnouncerSystem>();
            var proto = IoCManager.Resolve<IPrototypeManager>();

            switch (args.Length)
            {
                case 0:
                    shell.WriteError("Not enough arguments! Need at least 1.");
                    return;
                case 1:
                    announcer.SendAnnouncement(announcer.GetAnnouncementId("CommandReport"), Filter.Broadcast(),
                        args[0], "Central Command", Color.Gold);
                    break;
                case 2:
                    announcer.SendAnnouncement(announcer.GetAnnouncementId("CommandReport"), Filter.Broadcast(),
                        args[1], args[0], Color.Gold);
                    break;
                case 3:
                    announcer.SendAnnouncement(announcer.GetAnnouncementId(args[2]), Filter.Broadcast(), args[1],
                        args[0], Color.Gold);
                    break;
                case 4:
                    if (!proto.TryIndex(args[3], out AnnouncerPrototype? prototype))
                    {
                        shell.WriteError($"No announcer prototype with ID {args[3]} found!");
                        return;
                    }
                    announcer.SendAnnouncement(args[2], Filter.Broadcast(), args[1], args[0], Color.Gold, null,
                        prototype);
                    break;
            }

            shell.WriteLine("Sent!");
        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            switch (args.Length)
            {
                case 3:
                {
                    var list = new List<string>();

                    foreach (var prototype in IoCManager.Resolve<IPrototypeManager>()
                                 .EnumeratePrototypes<AnnouncerPrototype>()
                                 .SelectMany<AnnouncerPrototype, string>(p => p.Announcements.Select(a => a.ID)))
                    {
                        if (!list.Contains(prototype))
                            list.Add(prototype);
                    }

                    return CompletionResult.FromHintOptions(list, Loc.GetString("admin-announce-hint-sound"));
                }
                case 4:
                {
                    var list = new List<string>();

                    foreach (var prototype in IoCManager.Resolve<IPrototypeManager>()
                        .EnumeratePrototypes<AnnouncerPrototype>())
                    {
                        if (!list.Contains(prototype.ID))
                            list.Add(prototype.ID);
                    }

                    return CompletionResult.FromHintOptions(list, Loc.GetString("admin-announce-hint-voice"));
                }
                default:
                    return CompletionResult.Empty;
            }
        }
    }
}
