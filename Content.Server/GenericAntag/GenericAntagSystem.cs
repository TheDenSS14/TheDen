// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Vyacheslav Kovalevsky <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.GameTicking.Rules;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;

namespace Content.Server.GenericAntag;

/// <summary>
/// Handles adding objectives to <see cref="GenericAntagComponent"/>s.
/// Roundend summary is handled by <see cref="GenericAntagRuleSystem"/>.
/// </summary>
public sealed class GenericAntagSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly GenericAntagRuleSystem _genericAntagRule = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GenericAntagComponent, MindAddedMessage>(OnMindAdded);
    }

    private void OnMindAdded(EntityUid uid, GenericAntagComponent comp, MindAddedMessage args)
    {
        if (!TryComp<MindContainerComponent>(uid, out var mindContainer) || mindContainer.Mind == null)
            return;

        var mindId = mindContainer.Mind.Value;
        MakeAntag(uid, mindId, comp);
    }

    /// <summary>
    /// Turns a player into this antagonist.
    /// Does the same thing that having a mind added does, use for antag ctrl.
    /// </summary>
    public void MakeAntag(EntityUid uid, EntityUid mindId, GenericAntagComponent? comp = null, MindComponent? mind = null)
    {
        if (!Resolve(uid, ref comp) || !Resolve(mindId, ref mind))
            return;

        // only add the rule once
        if (comp.RuleEntity != null)
            return;

        // start the rule
        if (!_genericAntagRule.StartRule(comp.Rule, mindId, out comp.RuleEntity, out var rule))
            return;

        // let other systems know the antag was created so they can add briefing, roles, etc.
        // its important that this is before objectives are added since they may depend on roles added here
        var ev = new GenericAntagCreatedEvent(mindId, mind);
        RaiseLocalEvent(uid, ref ev);

        // add the objectives from the rule
        foreach (var id in rule.Objectives)
        {
            _mind.TryAddObjective(mindId, mind, id);
        }
    }

    /// <summary>
    /// DeltaV - used by paradox anomaly
    /// </summary>
    public void MakeAntag(EntityUid uid, string rule)
    {
        AddComp<GenericAntagComponent>(uid).Rule = rule;
    }
}

/// <summary>
/// Event raised on a player's entity after its simple antag rule is started.
/// Use this to add a briefing, roles, etc.
/// </summary>
[ByRefEvent]
public record struct GenericAntagCreatedEvent(EntityUid MindId, MindComponent Mind);
