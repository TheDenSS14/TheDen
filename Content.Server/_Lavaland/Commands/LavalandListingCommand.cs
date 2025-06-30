// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._Lavaland.Procedural.Systems;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Lavaland.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class LavalandListingCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entityManager = default!;

    public string Command => "listlavaland";

    public string Description => "Logs a list of all active lavaland maps into the console.";

    public string Help => "";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var lavalands = _entityManager.System<LavalandPlanetSystem>().GetLavalands();

        foreach (var (owner, comp) in lavalands)
        {
            var mapId = _entityManager.GetComponent<TransformComponent>(owner).MapID;
            var lavalandString = $"Type: {comp.PrototypeId} | MapID: {mapId} | MapUid: {owner} | Seed: {comp.Seed}";
            shell.WriteLine(lavalandString);
        }
    }
}
