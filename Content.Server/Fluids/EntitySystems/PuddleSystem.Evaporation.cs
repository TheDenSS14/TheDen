// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Psychpsyo
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Vordenburg
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 lzk
// SPDX-FileCopyrightText: 2025 Cami
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids.Components;

namespace Content.Server.Fluids.EntitySystems;

public sealed partial class PuddleSystem
{
    private static readonly TimeSpan EvaporationCooldown = TimeSpan.FromSeconds(1);

    private void OnEvaporationMapInit(Entity<EvaporationComponent> entity, ref MapInitEvent args)
    {
        entity.Comp.NextTick = _timing.CurTime + EvaporationCooldown;
    }

    private void UpdateEvaporation(EntityUid uid, Solution solution)
    {
        if (solution.GetTotalPrototypeQuantity(EvaporationReagents) > FixedPoint2.Zero)
        {
            if (!HasComp<EvaporationComponent>(uid))
            {
                var evaporation = AddComp<EvaporationComponent>(uid);
                evaporation.NextTick = _timing.CurTime + EvaporationCooldown;
            }
        }
        else
        {
            RemComp<EvaporationComponent>(uid);
        }
    }

    private void TickEvaporation()
    {
        var query = EntityQueryEnumerator<EvaporationComponent, PuddleComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();
        var curTime = _timing.CurTime;
        while (query.MoveNext(out var uid, out var evaporation, out var puddle))
        {
            if (evaporation.NextTick > curTime)
                continue;

            evaporation.NextTick += EvaporationCooldown;

            if (!_solutionContainerSystem.ResolveSolution(uid, puddle.SolutionName, ref puddle.Solution, out var puddleSolution))
                continue;

            var reagentTick = evaporation.EvaporationAmount * EvaporationCooldown.TotalSeconds;
            puddleSolution.SplitSolutionWithOnly(reagentTick, EvaporationReagents);
            _solutionContainerSystem.UpdateChemicals(puddle.Solution!.Value);

            // Despawn if we're done
            if (puddleSolution.Volume == FixedPoint2.Zero)
            {
                // Spawn a *sparkle*
                Spawn("PuddleSparkle", xformQuery.GetComponent(uid).Coordinates);
                QueueDel(uid);
            }
        }
    }
}
