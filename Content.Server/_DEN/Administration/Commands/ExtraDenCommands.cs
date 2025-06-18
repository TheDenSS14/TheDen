// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Silver <silvertorch5@gmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
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

    [AnyCommand]
    sealed class SelfDamageCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public string Command => "selfdamage";
        public string Description => Loc.GetString("self-damage-command-description");
        public string Help => Loc.GetString("self-damage-command-help", ("command", Command));

        private string[] _ignoreTypesGroups = new[]
        {
            "Decay",
            "Holy",
            "Immaterial",
            "Ion",
            "OrganFailure",
            "Electronic"
        };

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                return CompletionResult.FromHint(Loc.GetString("damage-command-arg-quantity"));
            }

            if (args.Length == 2)
            {
                var types = _prototypeManager.EnumeratePrototypes<DamageTypePrototype>()
                    .Where(d => !_ignoreTypesGroups.Contains(d.ID))
                    .Select(p => new CompletionOption(p.ID));

                var groups = _prototypeManager.EnumeratePrototypes<DamageGroupPrototype>()
                    .Where(d => !_ignoreTypesGroups.Contains(d.ID))
                    .Select(p => new CompletionOption(p.ID));

                return CompletionResult.FromHintOptions(types.Concat(groups).OrderBy(p => p.Value),
                    Loc.GetString("damage-command-arg-type"));
            }

            if (args.Length == 3)
            {
                // if type.Name is good enough for cvars, <bool> doesn't need localizing.
                return CompletionResult.FromHint("<bool>");
            }

            return CompletionResult.Empty;
        }

        private delegate void Damage(EntityUid entity, bool ignoreResistances);

        private bool TryParseDamageArgs(
            IConsoleShell shell,
            string[] args,
            [NotNullWhen(true)] out Damage? func)
        {
            if (!float.TryParse(args[0], out var amount))
            {
                shell.WriteLine(Loc.GetString("damage-command-error-quantity", ("arg", args[1])));
                func = null;
                return false;
            }

            var damageTypeOrGroup = args.Length > 2 ? args[1] : "Brute";

            if (_ignoreTypesGroups.Contains(damageTypeOrGroup))
            {
                shell.WriteLine("You cannot use this damage type or group.");
                func = null;
                return false;
            }

            if (amount <= 0)
            {
                shell.WriteLine("You cannot do negative or zero damage.");
                func = null;
                return false;
            }

            if (_prototypeManager.TryIndex<DamageGroupPrototype>(damageTypeOrGroup, out var damageGroup))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageGroup, amount);
                    _entManager.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };

                return true;
            }

            if (_prototypeManager.TryIndex<DamageTypePrototype>(damageTypeOrGroup, out var damageType))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageType, amount);
                    _entManager.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };

                return true;
            }

            shell.WriteLine(Loc.GetString("damage-command-error-type", ("arg", args[0])));
            func = null;
            return false;
        }

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 1 || args.Length > 3)
            {
                shell.WriteLine(Loc.GetString("damage-command-error-args"));
                return;
            }

            EntityUid? target;

            if (shell.Player?.AttachedEntity is { Valid: true } playerEntity)
                target = playerEntity;
            else
            {
                shell.WriteLine(Loc.GetString("self-damage-command-error-player"));
                return;
            }

            if (!TryParseDamageArgs(shell, args, out var damageFunc))
                return;

            bool ignoreResistances;
            if (args.Length == 3)
            {
                if (!bool.TryParse(args[2], out ignoreResistances))
                {
                    shell.WriteLine(Loc.GetString("damage-command-error-bool", ("arg", args[2])));
                    return;
                }
            }
            else
                ignoreResistances = false;

            damageFunc(target.Value, ignoreResistances);
        }
    }
}
