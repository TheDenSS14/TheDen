// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Roles.Jobs;
using Content.Server.WhiteDream.BloodCult.Gamerule;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Objectives.Components;

namespace Content.Server.WhiteDream.BloodCult.Objectives;

public sealed class KillTargetCultSystem : EntitySystem
{
    [Dependency] private readonly BloodCultRuleSystem _cultRule = default!;
    [Dependency] private readonly JobSystem _job = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<KillTargetCultComponent, ObjectiveAssignedEvent>(OnObjectiveAssigned);
        SubscribeLocalEvent<KillTargetCultComponent, ObjectiveAfterAssignEvent>(OnAfterAssign);
        SubscribeLocalEvent<KillTargetCultComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnObjectiveAssigned(Entity<KillTargetCultComponent> ent, ref ObjectiveAssignedEvent args)
    {
        var cultistRule = EntityManager.EntityQuery<BloodCultRuleComponent>().FirstOrDefault();
        if (cultistRule is null)
            return;

        if (cultistRule.OfferingTarget is null)
            _cultRule.SetRandomCultTarget(cultistRule);

        ent.Comp.Target = cultistRule.OfferingTarget;
    }

    private void OnAfterAssign(Entity<KillTargetCultComponent> ent, ref ObjectiveAfterAssignEvent args)
    {
        if (!ent.Comp.Target.HasValue || !ent.Owner.IsValid() || !HasComp<MetaDataComponent>(ent))
            return;

        _metaData.SetEntityName(ent, GetTitle(ent.Comp.Target.Value, ent.Comp.Title), args.Meta);
    }

    private void OnGetProgress(Entity<KillTargetCultComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var target = ent.Comp.Target;
        if (!target.HasValue)
        {
            args.Progress = 1f;
            return;
        }

        args.Progress = !HasComp<MobStateComponent>(target) || _mobState.IsDead(target.Value)
            ? 1f
            : 0f;
    }

    private string GetTitle(EntityUid target, string title)
    {
        var targetName = MetaData(target).EntityName;
        var jobName = _job.MindTryGetJobName(target);
        return Loc.GetString(title, ("targetName", targetName), ("job", jobName));
    }
}
