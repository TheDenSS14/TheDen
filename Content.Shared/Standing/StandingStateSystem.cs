// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Clyybber
// SPDX-FileCopyrightText: 2021 Galactic Chimp
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 Fishfish458
// SPDX-FileCopyrightText: 2022 Francesco
// SPDX-FileCopyrightText: 2022 Jacob Tong
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 fishfish458 <fishfish458>
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Aiden
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 gluesniffler
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2024 sleepyyapril <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>
// SPDX-FileCopyrightText: 2025 FoxxoTrystan
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Climbing.Systems;
using Content.Shared.Climbing.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Physics;
using Content.Shared.Rotation;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using System.Linq;
using Content.Shared.Body.Components;
using Content.Shared._DEN.Body;
using Content.Shared.Inventory;

namespace Content.Shared.Standing;

public sealed class StandingStateSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movement = default!;
    [Dependency] private readonly SharedBuckleSystem _buckle = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly ClimbSystem _climb = default!;

    // If StandingCollisionLayer value is ever changed to more than one layer, the logic needs to be edited.
    private const int StandingCollisionLayer = (int) CollisionGroup.MidImpassable;

    public bool IsDown(EntityUid uid, StandingStateComponent? standingState = null)
    {
        if (!Resolve(uid, ref standingState, false))
            return false;

        return standingState.CurrentState is StandingState.Lying or StandingState.GettingUp;
    }

    public bool Down(EntityUid uid, bool playSound = true, bool dropHeldItems = true,
        StandingStateComponent? standingState = null,
        AppearanceComponent? appearance = null,
        HandsComponent? hands = null,
        bool setDrawDepth = false)
    {
        // TODO: This should actually log missing comps...
        if (!Resolve(uid, ref standingState, false))
            return false;

        // Optional component.
        Resolve(uid, ref appearance, ref hands, false);

        if (standingState.CurrentState is StandingState.Lying or StandingState.GettingUp)
            return true;

        // This is just to avoid most callers doing this manually saving boilerplate
        // 99% of the time you'll want to drop items but in some scenarios (e.g. buckling) you don't want to.
        // We do this BEFORE downing because something like buckle may be blocking downing but we want to drop hand items anyway
        // and ultimately this is just to avoid boilerplate in Down callers + keep their behavior consistent.
        if (dropHeldItems && hands != null)
            RaiseLocalEvent(uid, new DropHandItemsEvent(), false);

        if (TryComp(uid, out BuckleComponent? buckle) && buckle.Buckled)
            return false;

        var msg = new DownAttemptEvent();
        RaiseLocalEvent(uid, msg, false);

        if (msg.Cancelled)
            return false;

        standingState.CurrentState = StandingState.Lying;
        Dirty(uid, standingState);
        RaiseLocalEvent(uid, new DownedEvent(), false);

        // Seemed like the best place to put it
        _appearance.SetData(uid, RotationVisuals.RotationState, RotationState.Horizontal, appearance);

        // Change collision masks to allow going under certain entities like flaps and tables
        if (TryComp(uid, out FixturesComponent? fixtureComponent))
            foreach (var (key, fixture) in fixtureComponent.Fixtures)
            {
                if ((fixture.CollisionMask & StandingCollisionLayer) == 0)
                    continue;

                standingState.ChangedFixtures.Add(key);
                _physics.SetCollisionMask(uid, key, fixture, fixture.CollisionMask & ~StandingCollisionLayer, manager: fixtureComponent);
            }

        // check if component was just added or streamed to client
        // if true, no need to play sound - mob was down before player could seen that
        if (standingState.LifeStage <= ComponentLifeStage.Starting)
            return true;

        if (playSound)
            _audio.PlayPredicted(standingState.DownSound, uid, null);

        _movement.RefreshMovementSpeedModifiers(uid);

        Climb(uid);

        return true;
    }

    public bool Stand(EntityUid uid,
        StandingStateComponent? standingState = null,
        AppearanceComponent? appearance = null,
        bool force = false)
    {
        // TODO: This should actually log missing comps...
        if (!Resolve(uid, ref standingState, false))
            return false;

        // Optional component.
        Resolve(uid, ref appearance, false);

        if (standingState.CurrentState is StandingState.Standing
            || TryComp(uid, out BuckleComponent? buckle)
            && buckle.Buckled && !_buckle.TryUnbuckle(uid, uid, buckleComp: buckle))
            return true;

        if (!force)
        {
            var msg = new StandAttemptEvent();
            RaiseLocalEvent(uid, msg, false);

            if (msg.Cancelled)
                return false;
        }

        standingState.CurrentState = StandingState.Standing;

        Dirty(uid, standingState);
        RaiseLocalEvent(uid, new StoodEvent(), false);

        _appearance.SetData(uid, RotationVisuals.RotationState, RotationState.Vertical, appearance);

        if (TryComp(uid, out FixturesComponent? fixtureComponent))
        {
            foreach (var key in standingState.ChangedFixtures)
            {
                if (fixtureComponent.Fixtures.TryGetValue(key, out var fixture))
                    _physics.SetCollisionMask(uid, key, fixture, fixture.CollisionMask | StandingCollisionLayer, fixtureComponent);
            }
        }
        standingState.ChangedFixtures.Clear();
        _movement.RefreshMovementSpeedModifiers(uid);

        Climb(uid);

        return true;
    }

    private void Climb(EntityUid uid)
    {
        _climb.ForciblyStopClimbing(uid);

        var entityDistances = new Dictionary<EntityUid, float>();

        foreach (var entity in _lookup.GetEntitiesIntersecting(uid)) // Floof - changed to GetEntitiesIntersecting to avoid climbing through walls
            if (HasComp<ClimbableComponent>(entity))
                entityDistances[entity] = (Transform(uid).Coordinates.Position - Transform(entity).Coordinates.Position).LengthSquared();

        if (entityDistances.Count > 0)
            _climb.ForciblySetClimbing(uid, entityDistances.OrderBy(e => e.Value).First().Key);
    }

    public void UpdateStanding(Entity<BodyComponent?> ent)
    {
        if (!Resolve(ent.Owner, ref ent.Comp, false))
            return;

        var ev = new CannotSupportStandingEvent(ent.Comp.LegEntities.Count);
        RaiseLocalEvent(ent.Owner, ev);

        if (ev.Forced || !ev.Cancelled)
            Down(ent.Owner, dropHeldItems: false);
    }
}


public sealed class DropHandItemsEvent : EventArgs { }

/// <summary>
///     Subscribe if you can potentially block a down attempt.
/// </summary>
public sealed class DownAttemptEvent : CancellableEntityEventArgs { }

/// <summary>
///     Subscribe if you can potentially block a stand attempt.
/// </summary>
public sealed class StandAttemptEvent : CancellableEntityEventArgs { }

/// <summary>
///     Raised when an entity becomes standing
/// </summary>
public sealed class StoodEvent : EntityEventArgs { }

/// <summary>
///     Raised when an entity is not standing
/// </summary>
public sealed class DownedEvent : EntityEventArgs { }

/// <summary>
///     Whether or not this entity can support standing up when they have less than the required amount of legs.
///     Cancel this event if the entity should be able to stand anyway.
/// </summary>
public sealed class CannotSupportStandingEvent : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;
    public int LegCount;
    public bool Forced = false;

    public CannotSupportStandingEvent(int legCount)
    {
        LegCount = legCount;
    }
}
