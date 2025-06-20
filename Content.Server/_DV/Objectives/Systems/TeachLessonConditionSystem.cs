// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DV.Objectives.Components;
using Content.Server.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;

namespace Content.Server._DV.Objectives.Systems;

/// <summary>
/// Handles teach a lesson condition logic, does not assign target.
/// </summary>
public sealed class TeachLessonConditionSystem : EntitySystem
{
    [Dependency] private readonly CodeConditionSystem _codeCondition = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Critical || args.OldMobState >= args.NewMobState
            || !TryComp<MindContainerComponent>(args.Target, out var mc) || mc.OriginalMind is not { } mindId)
            return;

        // If the attacker actually has the objective, we can just skip any enumeration outright.
        if (args.Origin is not null
            && HasComp<TeachLessonConditionComponent>(args.Origin)
            && TryComp<TargetObjectiveComponent>(args.Origin, out var targetComp)
            && targetComp.Target == mindId)
        {
            _codeCondition.SetCompleted(args.Origin!.Value);
            return;
        }

        // Get all TeachLessonConditionComponent entities
        var query = EntityQueryEnumerator<TeachLessonConditionComponent, TargetObjectiveComponent>();

        while (query.MoveNext(out var ent, out var conditionComp, out var targetObjective))
        {
            // Check if this objective's target matches the entity that died
            if (targetObjective.Target != mindId)
                continue;

            var userWorldPos = _transform.GetWorldPosition(ent);
            var targetWorldPos = _transform.GetWorldPosition(args.Target);

            var distance = (userWorldPos - targetWorldPos).Length();
            if (distance > conditionComp.MaxDistance
                || Transform(ent).MapID != Transform(args.Target).MapID)
                continue;

            _codeCondition.SetCompleted(ent);
        }
    }
}
