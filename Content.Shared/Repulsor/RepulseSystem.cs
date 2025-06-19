// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Interaction;
using Content.Shared.Standing;
using Content.Shared.Stunnable;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Repulsor;

public sealed class RepulseSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedStunSystem _stunSystem = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RepulseOnTouchComponent, StartCollideEvent>(HandleCollision);
        SubscribeLocalEvent<RepulseComponent, InteractHandEvent>(OnHandInteract);
    }

    private void HandleCollision(Entity<RepulseOnTouchComponent> touchRepulsor, ref StartCollideEvent args)
    {
        if (!TryComp(touchRepulsor, out RepulseComponent? repulse))
            return;

        Repulse((touchRepulsor.Owner, repulse), args.OtherEntity);
    }

    private void OnHandInteract(Entity<RepulseComponent> repulsor, ref InteractHandEvent args)
    {
        Repulse(repulsor, args.User);
    }

    public void Repulse(Entity<RepulseComponent> repulsor, EntityUid user)
    {
        var ev = new BeforeRepulseEvent(user);
        RaiseLocalEvent(repulsor, ev);
        if (ev.Cancelled)
            return;

        var direction = _transform.GetMapCoordinates(user).Position - _transform.GetMapCoordinates(repulsor).Position;
        var impulse = direction * repulsor.Comp.ForceMultiplier;

        _physics.ApplyLinearImpulse(user, impulse);
        _stunSystem.TryStun(user, repulsor.Comp.StunDuration, true);
        _stunSystem.TryKnockdown(user, repulsor.Comp.KnockdownDuration, true, DropHeldItemsBehavior.DropIfStanding);
    }
}
