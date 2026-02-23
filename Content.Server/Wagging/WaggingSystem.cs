// SPDX-FileCopyrightText: 2024 ArchPigeon
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Krunklehorn
// SPDX-FileCopyrightText: 2024 Morb
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._DEN.Markings;
using Content.Server.Actions;
using Content.Server.Humanoid;
using Content.Shared._DEN.Actions;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Mobs;
using Content.Shared.Toggleable;
using Content.Shared.Wagging;
using Robust.Shared.Prototypes;

namespace Content.Server.Wagging;

/// <summary>
/// Adds an action to toggle wagging animation for tails markings that supporting this
/// </summary>
public sealed class WaggingSystem : EntitySystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _humanoidAppearance = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WaggingComponent, MapInitEvent>(OnWaggingMapInit);
        SubscribeLocalEvent<WaggingComponent, ComponentShutdown>(OnWaggingShutdown);
        SubscribeLocalEvent<WaggingComponent, WaggingActionEvent>(OnWaggingToggle);
        SubscribeLocalEvent<WaggingComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnWaggingMapInit(EntityUid uid, WaggingComponent component, MapInitEvent args)
    {
        _actions.AddAction(uid, ref component.ActionEntity, component.Action, uid);
    }

    private void OnWaggingShutdown(EntityUid uid, WaggingComponent component, ComponentShutdown args)
    {
        _actions.RemoveAction(uid, component.ActionEntity);
    }

    private void OnWaggingToggle(EntityUid uid, WaggingComponent component, ref WaggingActionEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = TryToggleWagging(uid, wagging: component);
    }

    private void OnMobStateChanged(EntityUid uid, WaggingComponent component, MobStateChangedEvent args)
    {
        if (component.Wagging)
            TryToggleWagging(uid, wagging: component);
    }

    public bool TryToggleWagging(EntityUid uid, WaggingComponent? wagging = null, HumanoidAppearanceComponent? humanoid = null)
    {
        if (!Resolve(uid, ref wagging, ref humanoid))
            return false;

        if (!humanoid.MarkingSet.Markings.TryGetValue(MarkingCategories.Tail, out var markings))
            return false;

        if (markings.Count == 0)
            return false;

        for (var idx = 0; idx < markings.Count; idx++) // Animate all possible tails
        {
            var currentMarkingId = markings[idx].MarkingId;
            var opposite = _humanoidAppearance.GetOppositeAnimatedMarking(
                MarkingCategories.Tail,
                markings[idx].MarkingId,
                wagging.Suffix);


            if (opposite.Marking != null)
            {
                var ev = new AnimatedToggleEvent
                {
                    ActionEntity = wagging.ActionEntity,
                    OldMarkingId = currentMarkingId,
                    NewMarkingId = opposite.Marking
                };
                RaiseLocalEvent(uid, ev);
            }

            var isAnimated = _humanoidAppearance.SetAnimatedMarkingId(
                uid,
                MarkingCategories.Tail,
                idx,
                currentMarkingId,
                wagging.Suffix,
                humanoid: humanoid);

            if (isAnimated == null)
            {
                Log.Warning($"{ToPrettyString(uid)} tried toggling wagging for {currentMarkingId} but something went wrong.");
                continue;
            }

            wagging.Wagging = isAnimated.Value;
            _actions.SetToggled(wagging.ActionEntity, wagging.Wagging);
        }
        return true;
    }
}
