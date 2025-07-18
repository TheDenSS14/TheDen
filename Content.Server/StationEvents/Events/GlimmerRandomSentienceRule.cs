// SPDX-FileCopyrightText: 2023 Debug <sidneymaatman@gmail.com>
// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.GameTicking.Components;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Ghost.Roles.Components;
using Content.Shared.Abilities.Psionics;
using Content.Server.Speech.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.Mobs.Systems;

namespace Content.Server.StationEvents.Events;

/// <summary>
/// Glimmer version of the (removed) random sentience event
/// </summary>
internal sealed class GlimmerRandomSentienceRule : StationEventSystem<GlimmerRandomSentienceRuleComponent>
{
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly MetaDataSystem _metaDataSystem = default!;

    protected override void Started(EntityUid uid, GlimmerRandomSentienceRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        List<EntityUid> targetList = new();

        var query = EntityQueryEnumerator<SentienceTargetComponent>();
        while (query.MoveNext(out var target, out _))
        {
            if (HasComp<GhostTakeoverAvailableComponent>(target))
                continue;

            if (!_mobStateSystem.IsAlive(target))
                continue;

            targetList.Add(target);
        }

        RobustRandom.Shuffle(targetList);

        var toMakeSentient = RobustRandom.Next(1, component.MaxMakeSentient);

        foreach (var target in targetList)
        {
            if (toMakeSentient-- == 0)
                break;

            EntityManager.RemoveComponent<SentienceTargetComponent>(target);
            _metaDataSystem.SetEntityName(target, Loc.GetString("glimmer-event-awakened-prefix", ("entity", target)));
            var comp = EntityManager.EnsureComponent<GhostRoleComponent>(target);
            comp.RoleName = EntityManager.GetComponent<MetaDataComponent>(target).EntityName;
            comp.RoleDescription = Loc.GetString("station-event-random-sentience-role-description", ("name", comp.RoleName));
            RemComp<ReplacementAccentComponent>(target);
            RemComp<MonkeyAccentComponent>(target);
            EnsureComp<PsionicComponent>(target);
            EnsureComp<GhostTakeoverAvailableComponent>(target);
        }
    }
}
