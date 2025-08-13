// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._DEN.Administration.Commands;

[AnyCommand]
sealed class SelfDamageCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public string Command => "selfdamage";
    public string Description => Loc.GetString("self-damage-command-description");
    public string Help => Loc.GetString("self-damage-command-help", ("command", Command));

    private string _defaultDamageType = "Brute";

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
            shell.WriteError(Loc.GetString("damage-command-error-quantity", ("arg", args[0])));
            func = null;
            return false;
        }

        if (!args.TryGetValue(1, out var damageTypeOrGroup))
            damageTypeOrGroup = _defaultDamageType;

        if (_ignoreTypesGroups.Contains(damageTypeOrGroup))
        {
            shell.WriteError(Loc.GetString("damage-command-error-forbidden-damage-type", ("arg", damageTypeOrGroup)));
            func = null;
            return false;
        }

        if (amount <= 0)
        {
            shell.WriteError(Loc.GetString("damage-command-error-negative-or-zero"));
            func = null;
            return false;
        }

        DamageSpecifier? damageSpecifier = null;

        if (_prototypeManager.TryIndex<DamageGroupPrototype>(damageTypeOrGroup, out var damageGroup))
            damageSpecifier = new DamageSpecifier(damageGroup, amount);
        else if (_prototypeManager.TryIndex<DamageTypePrototype>(damageTypeOrGroup, out var damageType))
            damageSpecifier = new DamageSpecifier(damageType, amount);

        if (damageSpecifier == null)
        {
            shell.WriteLine(Loc.GetString("damage-command-error-type", ("arg", damageTypeOrGroup)));
            func = null;
            return false;
        }

        func = (entity, ignoreResistances) =>
        {
            var name = entity.ToString();
            if (_entManager.TryGetComponent<MetaDataComponent>(entity, out var meta))
                name = meta.EntityName;

            var damageResult = _entManager.System<DamageableSystem>()
                .TryChangeDamage(entity, damageSpecifier, ignoreResistances);

            if (damageResult == null)
            {
                shell.WriteError(Loc.GetString("damage-command-fail-message",
                    ("entity", name)));
                return;
            }

            shell.WriteLine(Loc.GetString("damage-command-success-message",
                ("damage", damageResult.GetTotal()),
                ("type", damageTypeOrGroup),
                ("entity", name)));
        };

        return true;
    }

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1 || args.Length > 3)
        {
            shell.WriteError(Loc.GetString("damage-command-error-args"));
            shell.WriteLine(Help);
            return;
        }

        EntityUid? target;

        if (shell.Player?.AttachedEntity is { Valid: true } playerEntity)
            target = playerEntity;
        else
        {
            shell.WriteError(Loc.GetString("self-damage-command-error-player"));
            return;
        }

        if (!TryParseDamageArgs(shell, args, out var damageFunc))
            return;

        bool ignoreResistances;
        if (args.Length == 3)
        {
            if (!bool.TryParse(args[2], out ignoreResistances))
            {
                shell.WriteError(Loc.GetString("damage-command-error-bool", ("arg", args[2])));
                return;
            }
        }
        else
            ignoreResistances = false;

        damageFunc(target.Value, ignoreResistances);
    }
}
