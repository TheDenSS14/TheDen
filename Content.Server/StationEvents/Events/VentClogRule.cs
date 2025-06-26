// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Pancake
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2022 T-Stalker
// SPDX-FileCopyrightText: 2022 moonheart08
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 Emisse
// SPDX-FileCopyrightText: 2023 Moony
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Kara
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.Station.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using JetBrains.Annotations;
using Robust.Shared.Random;
using System.Linq;
using Content.Server.Fluids.EntitySystems;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;

namespace Content.Server.StationEvents.Events;

[UsedImplicitly]
public sealed class VentClogRule : StationEventSystem<VentClogRuleComponent>
{
    [Dependency] private readonly SmokeSystem _smoke = default!;

    protected override void Started(EntityUid uid, VentClogRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        if (!TryGetRandomStation(out var chosenStation))
            return;

        // TODO: "safe random" for chems. Right now this includes admin chemicals.
        var allReagents = PrototypeManager.EnumeratePrototypes<ReagentPrototype>()
            .Where(x => !x.Abstract)
            .Select(x => x.ID).ToList();

        foreach (var (_, transform) in EntityManager.EntityQuery<GasVentPumpComponent, TransformComponent>())
        {
            if (CompOrNull<StationMemberComponent>(transform.GridUid)?.Station != chosenStation)
            {
                continue;
            }

            var solution = new Solution();

            if (!RobustRandom.Prob(0.33f))
                continue;

            // The Den start
            // var pickAny = RobustRandom.Prob(0.05f);
            var reagent = RobustRandom.Pick(component.SafeishVentChemicals); // removed the random chance of it being a completely random reagent as it would sometimes include consent-breaking ones
            // The Den end

            var weak = component.WeakReagents.Contains(reagent);
            var quantity = weak ? component.WeakReagentQuantity : component.ReagentQuantity;
            solution.AddReagent(reagent, quantity);

            var foamEnt = Spawn("Foam", transform.Coordinates);
            var spreadAmount = weak ? component.WeakSpread : component.Spread;
            _smoke.StartSmoke(foamEnt, solution, component.Time, spreadAmount);
            Audio.PlayPvs(component.Sound, transform.Coordinates);
        }
    }
}
