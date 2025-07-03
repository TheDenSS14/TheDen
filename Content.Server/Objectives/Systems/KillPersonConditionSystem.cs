// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Errant <35878406+Errant-4@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 flyingkarii <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.CriminalRecords.Systems;
using Content.Server.GameTicking;
using Content.Server.KillTracking;
using Content.Server._Floof.Traits.Components;
using Content.Server.Administration.Toolshed;
using Content.Server.Objectives.Components;
using Content.Server.Players;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Server.StationRecords.Systems;
using Content.Shared.CCVar;
using Content.Shared.CriminalRecords;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Roles.Jobs;
using Content.Shared.Security;
using Content.Shared.StationRecords;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Objectives.Systems;

/// <summary>
/// Handles kill person condition logic and picking random kill targets.
/// </summary>
public sealed class KillPersonConditionSystem : EntitySystem
{
    [Dependency] private readonly EmergencyShuttleSystem _emergencyShuttle = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedJobSystem _job = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly TargetObjectiveSystem _target = default!;

    private List<EntityUid> _wasKilled = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<KillPersonConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);

        SubscribeLocalEvent<PickRandomPersonComponent, ObjectiveAssignedEvent>(OnPersonAssigned);

        SubscribeLocalEvent<PickRandomHeadComponent, ObjectiveAssignedEvent>(OnHeadAssigned);

        SubscribeLocalEvent<RoundEndedEvent>(OnRoundEnd);
    }

    private void OnGetProgress(EntityUid uid, KillPersonConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        if (!_target.GetTarget(uid, out var target))
            return;

        args.Progress = GetProgress(target.Value, comp.RequireDead);
    }

    private void OnPersonAssigned(EntityUid uid, PickRandomPersonComponent comp, ref ObjectiveAssignedEvent args)
    {
        // invalid objective prototype
        if (!TryComp<TargetObjectiveComponent>(uid, out var target))
        {
            args.Cancelled = true;
            return;
        }

        // target already assigned
        if (target.Target != null)
            return;

        // no other humans to kill
        var allHumans = _mind.GetAliveHumans(args.MindId, comp.NeedsOrganic);
        if (allHumans.Count == 0)
        {
            args.Cancelled = true;
            return;
        }

        // Floofstation Edit Start
        if (comp.ObjectiveType > 0)
        {
            allHumans = MarkedList(allHumans, comp.ObjectiveType);
        }
        else
        {
            //Cancel the objective if no target type was declared.
            //Prevent potential edge cases of people that didn't opt in getting assigned.
            args.Cancelled = true;
            return;
            //Legacy code below
            /*          foreach (var mind in allHumans)
                                        if (_job.MindTryGetJob(mind, out _, out var prototype) && !prototype.CanBeAntagTarget)
                                           allHumans.Remove(mind); */
        }
        //If the culled list is now empty
        if (allHumans.Count == 0)
        {
            args.Cancelled = true;
            return;
        }
        // Floofstation Edit End

        _target.SetTarget(uid, _random.Pick(allHumans), target);
    }
    // Floofstation - added this entire function
    public HashSet<Entity<MindComponent>> MarkedList(HashSet<Entity<MindComponent>> markedList, ObjectiveTypes objType)
    {
        if (objType.HasFlag(ObjectiveTypes.TraitorKill))//Culls from the list of all alive minds anyone that hasn't opted into kill targetting.
        {
            foreach (var mind in markedList)
            {
                if (!TryComp<MarkedComponent>(mind.Comp.CurrentEntity, out var mComp) || !mComp.TargetType.HasFlag(ObjectiveTypes.TraitorKill))
                {
                    markedList.Remove(mind);
                }
            }
        }
        if (objType.HasFlag(ObjectiveTypes.TraitorTeach))//Culls from the list of all alive minds anyone that hasn't opted into teach targetting.
        {
            foreach (var mind in markedList)
            {
                if (!TryComp<MarkedComponent>(mind.Comp.CurrentEntity, out var mComp) || !mComp.TargetType.HasFlag(ObjectiveTypes.TraitorTeach))
                {
                    markedList.Remove(mind);
                }
            }
        }
        return markedList;
    }
    private void OnHeadAssigned(EntityUid uid, PickRandomHeadComponent comp, ref ObjectiveAssignedEvent args)
    {
        // invalid prototype
        if (!TryComp<TargetObjectiveComponent>(uid, out var target))
        {
            args.Cancelled = true;
            return;
        }

        // target already assigned
        if (target.Target != null)
            return;

        // no other humans to kill
        var allHumans = _mind.GetAliveHumans(exclude: args.MindId);
        if (allHumans.Count == 0)
        {
            args.Cancelled = true;
            return;
        }

        var allHeads = new List<EntityUid>();
        foreach (var mind in allHumans)
        {
            // RequireAdminNotify used as a cheap way to check for command department
            if (_job.MindTryGetJob(mind, out var prototype) && prototype.RequireAdminNotify)
                allHeads.Add(mind);
        }

        var pickCatch = new PickRandomPersonComponent();//Floofstation: Prepares for an edge case where there are no command members.
        if (allHeads.Count == 0)
        {
            pickCatch.ObjectiveType = ObjectiveTypes.TraitorTeach;//Floofstation: Sets the objective type
            OnPersonAssigned(uid, pickCatch, ref args);//Floofstation: Assigns a target from the Marked if possible
            return;//Floofstation: Escapes
            // allHeads = allHumans.Select(x => x.Owner).ToList(); // fallback to non-head target //Floofstation: Disabled to prevent non-consenting targets.
        }

        _target.SetTarget(uid, _random.Pick(allHeads), target);
    }

    private float GetProgress(EntityUid target, bool requireDead)
    {
        // deleted or gibbed or something, counts as dead
        if (!TryComp<MindComponent>(target, out var mind) || mind.OwnedEntity == null)
        {
            if (!_wasKilled.Contains(target)) _wasKilled.Add(target);
            return 1f;
        }

        // dead is success
        if (_mind.IsCharacterDeadIc(mind))
        {
            if (!_wasKilled.Contains(target)) _wasKilled.Add(target);
            return 1f;
        }

        // if the target was killed once and it isn't a head objective
        if (_wasKilled.Contains(target))
            return 1f;

        return 0f;
    }

    // Clear the wasKilled list on round end
    private void OnRoundEnd(RoundEndedEvent ev)
        => _wasKilled.Clear();
}
