// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.GameTicking.Components;
using Robust.Shared.Random;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Psionics.Glimmer;
using Content.Server.StationEvents.Components;

namespace Content.Server.StationEvents.Events;

internal sealed class GlimmerRevenantRule : StationEventSystem<GlimmerRevenantRuleComponent>
{
    [Dependency] private readonly IRobustRandom _random = default!;

    protected override void Started(EntityUid uid, GlimmerRevenantRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        List<EntityUid> glimmerSources = new();

        var query = EntityQueryEnumerator<GlimmerSourceComponent>();
        while (query.MoveNext(out var source, out _))
        {
            glimmerSources.Add(source);
        }

        if (glimmerSources.Count == 0)
            return;

        var coords = Transform(_random.Pick(glimmerSources)).Coordinates;

        Sawmill.Info($"Spawning revenant at {coords}");
        EntityManager.SpawnEntity(component.RevenantPrototype, coords);
    }
}
