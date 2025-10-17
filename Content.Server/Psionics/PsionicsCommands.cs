// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Abilities.Psionics;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Content.Server.Abilities.Psionics;
using Content.Server.Commands;
using Robust.Shared.Prototypes;
using Content.Shared.Psionics;
using Robust.Server.Player;


namespace Content.Server.Psionics;

[AdminCommand(AdminFlags.Logs)]
public sealed class ListPsionicsCommand : LocalizedEntityCommands
{
    public override string Command => "lspsionics";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        foreach (var (actor, psionic, meta) in entMan.EntityQuery<ActorComponent, PsionicComponent, MetaDataComponent>())
        {
            var powerList = new List<string>();
            foreach (var power in psionic.ActivePowers)
                powerList.Add(power.Name);

            shell.WriteLine(meta.EntityName + " (" + meta.Owner + ") - " + actor.PlayerSession.Name + powerList);
        }
    }
}

[AdminCommand(AdminFlags.Fun)]
public sealed class AddPsionicPowerCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilities = default!;

    public override string Command => "addpsionicpower";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("shell-need-exactly-one-argument"));
            return;
        }

        if (!EntityUid.TryParse(args[0], out var uid))
        {
            shell.WriteError(Loc.GetString("addpsionicpower-args-one-error"));
            return;
        }

        if (!_protoMan.TryIndex<PsionicPowerPrototype>(args[1], out var powerProto))
        {
            shell.WriteError(Loc.GetString("addpsionicpower-args-two-error"));
            return;
        }

        _entMan.EnsureComponent<PsionicComponent>(uid, out var psionic);
        _psionicAbilities.InitializePsionicPower(uid, powerProto, psionic);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromOptions(ContentCompletionHelper.PlayerAttachedEntities(_entMan, _playerMan));

        if (args.Length == 2)
        {
            var prototypes = _psionicAbilities.PsionicPowers();
            return CompletionResult.FromOptions(prototypes);
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Fun)]
public sealed class AddRandomPsionicPowerCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilities = default!;

    public override string Command => "addrandompsionicpower";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (!EntityUid.TryParse(args[0], out var uid))
        {
            shell.WriteError(Loc.GetString("addrandompsionicpower-args-one-error"));
            return;
        }

        _psionicAbilities.AddRandomPsionicPower(uid, true);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromOptions(ContentCompletionHelper.PlayerAttachedEntities(_entMan, _playerMan));

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Fun)]
public sealed class RemovePsionicPowerCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilities = default!;

    public override string Command => "removepsionicpower";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("shell-need-exactly-one-argument"));
            return;
        }

        if (!EntityUid.TryParse(args[0], out var uid))
        {
            shell.WriteError(Loc.GetString("removepsionicpower-args-one-error"));
            return;
        }

        if (!_protoMan.TryIndex<PsionicPowerPrototype>(args[1], out var powerProto))
        {
            shell.WriteError(Loc.GetString("removepsionicpower-args-two-error"));
            return;
        }

        if (!_entMan.TryGetComponent<PsionicComponent>(uid, out var psionicComponent))
        {
            shell.WriteError(Loc.GetString("removepsionicpower-not-psionic-error"));
            return;
        }

        if (!psionicComponent.ActivePowers.Contains(powerProto))
        {
            shell.WriteError(Loc.GetString("removepsionicpower-not-contains-error"));
            return;
        }

        _psionicAbilities.RemovePsionicPower(uid, psionicComponent, powerProto, true);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromOptions(ContentCompletionHelper.PlayerAttachedEntities(_entMan, _playerMan));

        if (args.Length == 2)
        {
            var prototypes = _psionicAbilities.PsionicPowers();
            return CompletionResult.FromOptions(prototypes);
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Fun)]
public sealed class RemoveAllPsionicPowersCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilities = default!;

    public override string Command => "removeallpsionicpowers";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var psionicPowers = entMan.System<PsionicAbilitiesSystem>();

        if (!EntityUid.TryParse(args[0], out var uid))
        {
            shell.WriteError(Loc.GetString("removeallpsionicpowers-args-one-error"));
            return;
        }

        if (!entMan.HasComponent<PsionicComponent>(uid))
        {
            shell.WriteError(Loc.GetString("removeallpsionicpowers-not-psionic-error"));
            return;
        }

        psionicPowers.RemoveAllPsionicPowers(uid);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromOptions(ContentCompletionHelper.PlayerAttachedEntities(_entMan, _playerMan));

        return CompletionResult.Empty;
    }
}
