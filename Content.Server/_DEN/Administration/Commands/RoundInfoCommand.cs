// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Silver
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Server.Administration;
using Content.Server.Administration.Systems;
using Content.Server.GameTicking;
using Content.Shared._DEN.Species;
using Content.Shared.Administration;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.HeightAdjust;
using Content.Shared.Humanoid;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._DEN.Administration.Commands
{
    [AnyCommand]
    sealed class RoundInfoCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public string Command => "roundinfo";
        public string Description => "Prints the current round information.";
        public string Help => "roundinfo";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var sysManager = IoCManager.Resolve<IEntitySystemManager>();
            var gameTicker = sysManager.GetEntitySystem<GameTicker>();

            var roundId = gameTicker.RoundId;
            var characters = gameTicker.PlayersJoinedRoundNormally;
            var players = _playerManager.PlayerCount;
            var maxPlayers = _playerManager.MaxPlayers;
            var entities = _entManager.EntityCount;

            shell.WriteLine($"Round ID: {roundId}");
            shell.WriteLine($"Players: {players}/{maxPlayers}");
            shell.WriteLine($"Characters: {characters}");
            shell.WriteLine($"Entity count: {entities}");
        }
    }
}
