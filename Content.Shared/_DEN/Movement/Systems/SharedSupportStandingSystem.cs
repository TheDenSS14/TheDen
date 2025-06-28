// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Body;
using Content.Shared._DEN.Movement.Components;
using Content.Shared.Hands;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Standing;

namespace Content.Shared._DEN.Movement.Systems;

public abstract class SharedSupportStandingSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _itemToggle = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<AlwaysSupportStandingComponent, ComponentShutdown>(OnLoseSupport);
        SubscribeLocalEvent<AlwaysSupportStandingComponent, CannotSupportStandingEvent>(AlwaysSupportStanding);

        SubscribeLocalEvent<HeldSupportStandingComponent, ComponentShutdown>(OnLoseSupport);
        SubscribeLocalEvent<HeldSupportStandingComponent, GotUnequippedHandEvent>(OnGotUnequippedHand);
        SubscribeLocalEvent<HeldSupportStandingComponent, ItemToggledEvent>(OnToggled);
        SubscribeLocalEvent<HeldSupportStandingComponent,
            HeldRelayedEvent<CannotSupportStandingEvent>>(SupportStandingWhenHeld);

        SubscribeLocalEvent<WornSupportStandingComponent, ComponentShutdown>(OnLoseSupport);
        SubscribeLocalEvent<WornSupportStandingComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<WornSupportStandingComponent,
            InventoryRelayedEvent<CannotSupportStandingEvent>>(SupportStandingWhenWorn);
    }

    private void OnLoseSupport(EntityUid uid, SupportStandingComponent comp, ref ComponentShutdown args)
        => LoseStandingSupport(uid);

    private void OnGotUnequippedHand(Entity<HeldSupportStandingComponent> ent, ref GotUnequippedHandEvent args)
        => LoseStandingSupport(args.User);

    private void OnGotUnequipped(Entity<WornSupportStandingComponent> ent, ref GotUnequippedEvent args)
        => LoseStandingSupport(args.Equipee);

    private void LoseStandingSupport(EntityUid uid)
    {
        var ev = new StandingSupportLostEvent();
        RaiseLocalEvent(uid, ev);

        _standing.UpdateStanding(uid);
    }

    private void OnToggled(EntityUid uid, SupportStandingComponent comp, ref ItemToggledEvent args)
    {
        if (args.User != null)
            _standing.UpdateStanding(args.User.Value);
    }

    protected void AlwaysSupportStanding(Entity<AlwaysSupportStandingComponent> ent,
        ref CannotSupportStandingEvent args)
        => TrySupportStanding(null, ent.Comp, ref args);

    protected void SupportStandingWhenHeld(Entity<HeldSupportStandingComponent> ent,
        ref HeldRelayedEvent<CannotSupportStandingEvent> args)
        => TrySupportStanding(ent.Owner, ent.Comp, ref args.Args);

    protected void SupportStandingWhenWorn(Entity<WornSupportStandingComponent> ent,
        ref InventoryRelayedEvent<CannotSupportStandingEvent> args)
        => TrySupportStanding(ent.Owner, ent.Comp, ref args.Args);

    /// <summary>
    ///     Allow an entity to stand if all conditions are met. Namely, the entity must have enough legs.
    /// </summary>
    /// <param name="uid">The entity used to provide standing support. NOT the entity that is receiving it.</param>
    /// <param name="comp">The component representing the supporting entity's ability to provide support.</param>
    /// <param name="args">Event arguments determining if the supported entity can stand.</param>
    private void TrySupportStanding(EntityUid? uid, SupportStandingComponent comp, ref CannotSupportStandingEvent args)
    {
        if (args.LegCount >= comp.MinimumLegCount &&
            (uid == null || _itemToggle.IsActivated(uid.Value)))
            args.Cancel();
    }
}

public sealed class StandingSupportLostEvent : EntityEventArgs
{ }
