// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DEN.Body;
using Content.Shared.Body.Systems;
using Content.Shared.Buckle.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Content.Shared.Throwing;

namespace Content.Shared.Traits.Assorted.Systems;

public sealed class LegsParalyzedSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifierSystem = default!;
    [Dependency] private readonly StandingStateSystem _standingSystem = default!;
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<Components.LegsParalyzedComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, BuckledEvent>(OnBuckled);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, UnbuckledEvent>(OnUnbuckled);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, ThrowPushbackAttemptEvent>(OnThrowPushbackAttempt);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, UpdateCanMoveEvent>(OnUpdateCanMoveEvent);
        SubscribeLocalEvent<Components.LegsParalyzedComponent, CannotSupportStandingEvent>(OnCannotSupportStanding);
    }

    private void OnStartup(EntityUid uid, Components.LegsParalyzedComponent component, ComponentStartup args)
    {
        // TODO: In future probably must be surgery related wound
        _movementSpeedModifierSystem.ChangeBaseSpeed(uid, 0, 0, 20);
    }

    private void OnShutdown(EntityUid uid, Components.LegsParalyzedComponent component, ComponentShutdown args)
    {
        _standingSystem.Stand(uid);
        _bodySystem.UpdateMovementSpeed(uid);
    }

    private void OnBuckled(EntityUid uid, Components.LegsParalyzedComponent component, ref BuckledEvent args) => _standingSystem.Stand(uid);

    private void OnUnbuckled(EntityUid uid, Components.LegsParalyzedComponent component, ref UnbuckledEvent args) => _standingSystem.Down(uid);

    private void OnUpdateCanMoveEvent(EntityUid uid, Components.LegsParalyzedComponent component, UpdateCanMoveEvent args) => args.Cancel();

    private void OnThrowPushbackAttempt(EntityUid uid, Components.LegsParalyzedComponent component, ThrowPushbackAttemptEvent args) => args.Cancel();

    private void OnCannotSupportStanding(Entity<Components.LegsParalyzedComponent> ent,
        ref CannotSupportStandingEvent args)
        => args.Forced = true;
}
