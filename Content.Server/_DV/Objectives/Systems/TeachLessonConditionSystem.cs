// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server._DV.Objectives.Components;
using Content.Server.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared.Mind;
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
        if (args.NewMobState != MobState.Dead)
            return;

        if (!TryComp<MindContainerComponent>(args.Target, out var mc) || mc.OriginalMind is not {} victimMindId)
            return;

        var victimPos = _transform.GetWorldPosition(args.Target);
        var victimMapId = Transform(args.Target).MapID;

        var mindQuery = EntityQueryEnumerator<MindComponent>();
        while (mindQuery.MoveNext(out _, out var mind))
        {
            if (mind.OwnedEntity is not {} ownerBody)
                continue;

            if (Transform(ownerBody).MapID != victimMapId)
                continue;

            var ownerPos = _transform.GetWorldPosition(ownerBody);

            foreach (var objective in mind.Objectives)
            {
                if (!TryComp<TeachLessonConditionComponent>(objective, out var condition))
                    continue;

                if (!TryComp<TargetObjectiveComponent>(objective, out var target))
                    continue;

                if (target.Target != victimMindId)
                    continue;

                if ((ownerPos - victimPos).Length() > condition.MaxDistance)
                    continue;

                _codeCondition.SetCompleted(objective);
            }
        }
    }
}
