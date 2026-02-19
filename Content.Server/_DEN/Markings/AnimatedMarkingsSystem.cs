// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server.Actions;
using Content.Server.Humanoid;
using Content.Shared._DEN.CCVars;
using Content.Shared._DEN.Markings;
using Content.Shared.CCVar;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Toggleable;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Markings;


/// <summary>
/// This handles animating markings between colors.
/// </summary>
public sealed class AnimatedMarkingsSystem : EntitySystem
{
    [Dependency] private readonly ActionsSystem _actionsSystem = null!;
    [Dependency] private readonly IConfigurationManager _cfgManager = null!;
    [Dependency] private readonly IEntityManager _entityManager = null!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = null!;
    [Dependency] private readonly HumanoidAppearanceSystem _humanoidAppearanceSystem = null!;

    private bool _glowAnimationEnabled;

    public override void Initialize()
    {
        base.Initialize();

        _glowAnimationEnabled = _cfgManager.GetCVar(DenCCVars.MarkingGlowAnimation);
        Subs.CVar(_cfgManager, DenCCVars.MarkingGlowAnimation, OnMarkingGlowAnimationToggled);

        SubscribeLocalEvent<AnimatedMarkingsComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<AnimatedMarkingsComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<AnimatedMarkingsComponent, ToggleActionEvent>(OnToggleAction);
    }

    private void OnMarkingGlowAnimationToggled(bool newValue) => _glowAnimationEnabled = newValue;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_glowAnimationEnabled)
            return;

        var query = _entityManager.EntityQueryEnumerator<AnimatedMarkingsComponent>();

        while (query.MoveNext(out var uid, out var animatedMarking))
        {
            if (!animatedMarking.IsAnimating)
                continue;

            foreach (var markingIndex in animatedMarking.MarkingIds)
            {
                HandleMarkings((uid, animatedMarking), markingIndex);
            }

            if (animatedMarking.StopAnimatingNextFrame)
            {
                animatedMarking.StopAnimatingNextFrame = false;
                animatedMarking.IsAnimating = false;
            }
        }
    }

    private void OnMapInit(Entity<AnimatedMarkingsComponent> ent, ref MapInitEvent args) =>
        _actionsSystem.AddAction(ent, ref ent.Comp.ActionEntity, ent.Comp.ActionPrototype, ent);

    private void OnShutdown(Entity<AnimatedMarkingsComponent> ent, ref ComponentShutdown args) =>
        _actionsSystem.RemoveAction(ent, ent.Comp.ActionEntity);

    private void OnToggleAction(Entity<AnimatedMarkingsComponent> ent, ref ToggleActionEvent args)
    {
        if (args.Handled)
            return;

        _actionsSystem.SetToggled(ent.Comp.ActionEntity, !ent.Comp.IsAnimating);

        if (!ent.Comp.IsAnimating)
        {
            ent.Comp.IsAnimating = true;
            return;
        }

        ent.Comp.StopAnimatingNextFrame = !ent.Comp.StopAnimatingNextFrame;
        args.Handled = true;
    }

    public void DoDebugAnimation(Entity<HumanoidAppearanceComponent> ent)
    {
        var (uid, humanoid) = ent;
        var animatedMarking = EnsureComp<AnimatedMarkingsComponent>(uid);

        foreach (var markingCategory in Enum.GetValues<MarkingCategories>())
        {
            var hasMarkings = humanoid.MarkingSet.Markings.TryGetValue(markingCategory, out var markings);

            if (!hasMarkings || markings == null || markings.Count == 0)
                continue;

            foreach (var marking in markings)
            {
                if (!_prototypeManager.TryIndex<MarkingPrototype>(marking.MarkingId, out var markingPrototype)
                    || markingPrototype.Shader != "unshaded")
                    continue;

                var lastColor = marking.MarkingColors.LastOrDefault();

                if (lastColor == default)
                    continue;

                var id = marking.MarkingId;

                animatedMarking.MarkingIds.Add(marking.MarkingId);
                animatedMarking.MarkingIdToIndex[id] = 0;
                animatedMarking.InitialColors[id] = lastColor;
                animatedMarking.GoalColors[id] = Color.InterpolateBetween(lastColor, Color.Black, animatedMarking.InterpolationLambda);
                animatedMarking.CurrentStates[id] = 0;
                animatedMarking.MarkingCategories[id] = markingCategory;
                animatedMarking.IsReversing[id] = false;
                animatedMarking.IgnoreColorIndices[id] = new();

                for (var i = 0; i < marking.MarkingColors.Count; i++)
                {
                    var color = marking.MarkingColors[i];

                    if (color == lastColor)
                        continue;

                    animatedMarking.IgnoreColorIndices[id].Add(i);
                }

                animatedMarking.IsReady[id] = true;
            }
        }
    }


    private void HandleMarkings(
        Entity<AnimatedMarkingsComponent> ent,
        ProtoId<MarkingPrototype> markingId,
        HumanoidAppearanceComponent? humanoid = null
    )
    {
        if (!Resolve(ent.Owner, ref humanoid, false))
            return;

        var (uid, animatedMarking) = ent;

        if (!animatedMarking.IsReady.TryGetValue(markingId, out var ready) || !ready)
            return;

        var initialColor = animatedMarking.InitialColors[markingId];
        var endGoal = animatedMarking.GoalColors[markingId];
        var lastState = animatedMarking.CurrentStates[markingId];
        var markingCategories = animatedMarking.MarkingCategories[markingId];
        var reversing = animatedMarking.IsReversing[markingId];
        var ignoreIds = animatedMarking.IgnoreColorIndices[markingId];
        var markingIndex = animatedMarking.MarkingIdToIndex[markingId];

        var markings = humanoid.MarkingSet.Markings[markingCategories];
        var marking = markings.FirstOrDefault(m => m.MarkingId == markingId);

        if (marking == null)
            return;

        var stateChange = reversing ? -ent.Comp.Step : ent.Comp.Step;
        var state = Math.Min(lastState + stateChange, 100);

        state = Math.Max(0, state);

        var targetState = reversing ? 0 : 100;
        var shouldSwap = Math.Abs(targetState - state) < 0.4;
        var interpolated = Color.InterpolateBetween(initialColor, endGoal, state / 100);

        if (ent.Comp.StopAnimatingNextFrame)
        {
            interpolated = initialColor;
            state = 0;
        }

        var markingColors = marking.MarkingColors;
        var newColors = new List<Color>();

        for (var i = 0; i < markingColors.Count; i++)
        {
            if (ignoreIds.Contains(i))
            {
                newColors.Add(markingColors[i]);
                continue;
            }

            newColors.Add(interpolated);
        }

        _humanoidAppearanceSystem.SetMarkingColor(uid, markingCategories, markingIndex, newColors, humanoid);

        animatedMarking.CurrentStates[markingId] = state;
        animatedMarking.LastState = state;

        if (shouldSwap)
        {
            animatedMarking.CurrentStates[markingId] = targetState;
            animatedMarking.IsReversing[markingId] = !reversing;
        }
    }
}
