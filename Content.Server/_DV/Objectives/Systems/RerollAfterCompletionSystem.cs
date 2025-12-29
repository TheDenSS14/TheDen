// SPDX-FileCopyrightText: 2025 William Lemon
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server._DV.Objectives.Components;
using Content.Server.Objectives;
using Content.Server.Objectives.Components;
using Content.Server.Roles.Jobs;
using Content.Shared._DEN.Actions;
using Content.Shared.Actions;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Objectives.Systems;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.Objectives.Systems;

public sealed class RerollAfterCompletionSystem : EntitySystem
{
    [Dependency] private readonly ObjectivesSystem _objectives = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly JobSystem _job = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    private readonly EntProtoId _skiaReapObjective = "SkiaReapObjective";
    private readonly LocId _skiaRerollObjectiveMessage = "objective-action-reroll-message";
    private readonly HashSet<RerollAfterCompletionComponent> _objectivesToAdd = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RerollAfterCompletionComponent, ObjectiveAfterAssignEvent>(OnObjectiveAfterAssign);
        SubscribeLocalEvent<RerollSkiaObjectivesActionEvent>(OnRerollObjectives);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        _objectivesToAdd.Clear();
        var query = EntityQueryEnumerator<RerollAfterCompletionComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.Rerolling) // If already rerolled, skip.
                continue;

            if (!HasComp<ObjectiveComponent>(uid))
                continue; // If the entity doesn't have an ObjectiveComponent, skip.

            if (!TryComp<MindComponent>(component.MindUid, out var mind))
                continue; // If the mind component is missing, skip.

            // Check that this objective has been completed.
            if (!_objectives.IsCompleted(uid, new(component.MindUid, mind)))
                continue;

            // Destroy this commponent as it is no longer needed, and this will speed up the next check.
            RemCompDeferred<RerollAfterCompletionComponent>(uid);

            component.Rerolling = true;

            // I'd be a lot happier if I could do all the rerolling here
            // But creating the new objective causes the Query to freak out
            // And I need the objective to do everything else.
            _objectivesToAdd.Add(component);
        }

        foreach (var component in _objectivesToAdd)
        {
            RerollObjective(component);
        }
    }

    private void OnRerollObjectives(RerollSkiaObjectivesActionEvent args)
    {
        if (!_mind.TryGetMind(args.Performer, out var mindId, out var mindComp))
            return;

        var mind = new Entity<MindComponent>(mindId, mindComp);
        var objectivesToRoll = 0;

        for (var i = 0; i < mind.Comp.Objectives.Count; i++)
        {
            var objective = mind.Comp.Objectives[i];

            if (!TryComp(objective, out MetaDataComponent? metaData))
                continue;

            if (metaData.EntityPrototype == null || metaData.EntityPrototype.ID != _skiaReapObjective)
                continue;

            _mind.TryRemoveObjective(mind, mind.Comp, i);
            objectivesToRoll++;
        }

        for (var i = 0; i < objectivesToRoll; i++)
            RerollObjective(mind.Owner, _skiaReapObjective);

        _popup.PopupEntity(
            Loc.GetString(_skiaRerollObjectiveMessage),
            args.Performer,
            args.Performer,
            PopupType.Large);
        args.Handled = true;
    }

    private void RerollObjective(RerollAfterCompletionComponent component) =>
        RerollObjective(component.MindUid, component.RerollObjectivePrototype, component.RerollObjectiveMessage);

    public void RerollObjective(
        EntityUid mind,
        EntProtoId rerollObjectivePrototype,
        LocId? rerollObjectiveMessage = null)
    {
        if (!TryComp<MindComponent>(mind, out var mindComponent))
            return;

        // Create a new objective with the specified prototype.
        if (_objectives.TryCreateObjective(mind, mindComponent, rerollObjectivePrototype) is not { } newObjUid)
            return;

        _mind.AddObjective(mind, mindComponent, newObjUid);

        if (rerollObjectiveMessage is null)
            return;

        var bodyUid = mindComponent.CurrentEntity ?? mind;

        // Check if this has a target component, and if so, get its name for Localization.
        if (TryComp<TargetObjectiveComponent>(newObjUid, out var targetComp) && TryComp<MindComponent>(targetComp.Target, out var targetMindComp))
        {
            var newTarget = targetMindComp.CharacterName ?? "Unknown";
            var targetJob = _job.MindTryGetJobName(targetComp.Target);
            _popup.PopupEntity(Loc.GetString(rerollObjectiveMessage, ("targetName", newTarget), ("job", targetJob)), bodyUid, bodyUid, PopupType.Large);
        }
        else
        {
            _popup.PopupEntity(Loc.GetString(rerollObjectiveMessage), bodyUid, bodyUid, PopupType.Large);
        }
    }

    private void OnObjectiveAfterAssign(EntityUid uid, RerollAfterCompletionComponent comp, ref ObjectiveAfterAssignEvent args)
    {
        // If the objective is assigned, we can set the mind UID.
        comp.MindUid = args.MindId;
    }
}
