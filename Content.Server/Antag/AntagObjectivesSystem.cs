// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Antag.Components;
using Content.Server.Objectives;
using Content.Shared.Mind;
using Content.Shared.Objectives.Systems;
using Content.Shared.Roles;


namespace Content.Server.Antag;

/// <summary>
/// Adds fixed objectives to an antag made with <c>AntagObjectivesComponent</c>.
/// </summary>
public sealed class AntagObjectivesSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mind = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AntagObjectivesComponent, AfterAntagEntitySelectedEvent>(OnAntagSelected);
        SubscribeLocalEvent<RoleAddedEvent>(OnRoleAdded);
    }


    private void OnAntagSelected(Entity<AntagObjectivesComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        if (!_mind.TryGetMind(args.Session, out var mindId, out var mind))
        {
            Log.Error($"Antag {ToPrettyString(args.EntityUid):player} was selected by {ToPrettyString(ent):rule} but had no mind attached!");
            return;
        }

        foreach (var id in ent.Comp.Objectives)
        {
            _mind.TryAddObjective(mindId, mind, id);
        }
    }

    private void OnRoleAdded(RoleAddedEvent args)
    {
        foreach (var roleId in args.Mind.MindRoles)
        {
            if (!TryComp<AntagObjectivesComponent>(roleId, out var objectivesComp))
                continue;

            if (!TryComp<MindRoleComponent>(roleId, out var mindRoleComp))
                continue;

            if (mindRoleComp.Mind.Owner != args.MindId)
                continue;

            foreach (var id in objectivesComp.Objectives)
            {
                if (_mind.TryFindObjective((args.MindId, args.Mind), id, out _))
                    continue;
                _mind.TryAddObjective(args.MindId, args.Mind, id);
            }
        }
    }
}