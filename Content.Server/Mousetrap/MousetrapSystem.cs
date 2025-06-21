// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Memeji <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Damage.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Popups;
using Content.Shared.Abilities;
using Content.Shared.Interaction.Events;
using Content.Shared.Mousetrap;
using Content.Shared.StepTrigger.Systems;
using Robust.Shared.Physics.Components;

namespace Content.Server.Mousetrap;

public sealed class MousetrapSystem : EntitySystem
{
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<MousetrapComponent, UseInHandEvent>(OnUseInHand);
        // SubscribeLocalEvent<MousetrapComponent, BeforeDamageUserOnTriggerEvent>(BeforeDamageOnTrigger);
        SubscribeLocalEvent<MousetrapComponent, StepTriggerAttemptEvent>(OnStepTriggerAttempt);
        SubscribeLocalEvent<MousetrapComponent, TriggerEvent>(OnTrigger);
    }

    private void OnUseInHand(EntityUid uid, MousetrapComponent component, UseInHandEvent args)
    {
        component.IsActive = !component.IsActive;
        _popupSystem.PopupEntity(component.IsActive
            ? Loc.GetString("mousetrap-on-activate")
            : Loc.GetString("mousetrap-on-deactivate"),
            uid,
            args.User);

        UpdateVisuals(uid);
    }

    private void OnStepTriggerAttempt(EntityUid uid, MousetrapComponent component, ref StepTriggerAttemptEvent args)
    {
        // DeltaV: Entities with this component always trigger mouse traps, even if wearing shoes
        if (HasComp<AlwaysTriggerMousetrapComponent>(args.Tripper))
            args.Cancelled = false;

        args.Continue |= component.IsActive;
    }

    private void BeforeDamageOnTrigger(EntityUid uid, MousetrapComponent component, BeforeDamageUserOnTriggerEvent args)
    {
        if (!TryComp<PhysicsComponent>(args.Tripper, out var physics)
            || physics.Mass is 0)
            return;

        // The idea here is inverse,
        // Small - big damage,
        // Large - small damage
        // Yes, I punched numbers into a calculator until the graph looked right
        var scaledDamage = -50 * MathF.Atan(physics.Mass - component.MassBalance) + 25 * MathF.PI;
        args.Damage *= scaledDamage;
    }

    private void OnTrigger(EntityUid uid, MousetrapComponent component, TriggerEvent args)
    {
        component.IsActive = false;
        UpdateVisuals(uid);
    }

    private void UpdateVisuals(EntityUid uid, MousetrapComponent? mousetrap = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref mousetrap, ref appearance, false))
            return;

        _appearance.SetData(uid, MousetrapVisuals.Visual,
            mousetrap.IsActive ? MousetrapVisuals.Armed : MousetrapVisuals.Unarmed, appearance);
    }
}
