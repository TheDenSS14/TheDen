// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Abilities.Psionics;
using Content.Shared.Actions.Events;

namespace Content.Server.Abilities.Psionics;

public sealed partial class AnomalyPowerSystem
{
    private void DoPuddleAnomalyEffects(EntityUid uid, PsionicComponent component, AnomalyPowerActionEvent args, bool overcharged = false)
    {
        if (args.Puddle is null)
            return;

        if (overcharged)
            PuddleSupercrit(uid, args);
        else PuddlePulse(uid, component, args);
    }

    private void PuddleSupercrit(EntityUid uid, AnomalyPowerActionEvent args)
    {
        var puddle = args.Puddle!.Value;
        if (!_solutionContainer.TryGetSolution(uid, puddle.Solution, out _, out var sol))
            return;

        var xform = Transform(uid);
        _puddle.TrySpillAt(xform.Coordinates, sol, out _);
    }

    private void PuddlePulse(EntityUid uid, PsionicComponent component, AnomalyPowerActionEvent args)
    {
        var puddle = args.Puddle!.Value;
        if (!_solutionContainer.TryGetSolution(uid, puddle.Solution, out var sol, out _))
            return;

        var xform = Transform(uid);
        var puddleSol = _solutionContainer.SplitSolution(sol.Value, puddle.MaxPuddleSize * component.CurrentAmplification);
        _puddle.TrySplashSpillAt(uid, xform.Coordinates, puddleSol, out _);
    }
}