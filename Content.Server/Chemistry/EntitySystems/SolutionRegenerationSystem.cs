// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Server.Chemistry.EntitySystems;

public sealed class SolutionRegenerationSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<SolutionRegenerationComponent>();
        while (query.MoveNext(out var uid, out var regen))
        {
            if (_timing.CurTime < regen.NextRegenTime
                || !TryComp(uid, out SolutionContainerManagerComponent? manager))
                continue;

            // timer ignores if its full, it's just a fixed cycle
            regen.NextRegenTime = _timing.CurTime + regen.Duration;
            if (_solutionContainer.ResolveSolution((uid, manager), regen.SolutionName, ref regen.SolutionRef, out var solution))
            {
                var amount = FixedPoint2.Min(solution.AvailableVolume, regen.Generated.Volume);
                if (amount <= FixedPoint2.Zero)
                    continue;

                // dont bother cloning and splitting if adding the whole thing
                Solution generated;
                if (amount == regen.Generated.Volume)
                {
                    generated = regen.Generated;
                }
                else
                {
                    generated = regen.Generated.Clone().SplitSolution(amount);
                }

                _solutionContainer.TryAddSolution(regen.SolutionRef.Value, generated);
            }
        }
    }
}
