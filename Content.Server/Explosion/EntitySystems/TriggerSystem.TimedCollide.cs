// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.Explosion.Components;
using Content.Server.Explosion.EntitySystems;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Physics.Events;

namespace Content.Server.Explosion.EntitySystems;

public sealed partial class TriggerSystem
{
    private void InitializeTimedCollide()
    {
        SubscribeLocalEvent<TriggerOnTimedCollideComponent, StartCollideEvent>(OnTimerCollide);
        SubscribeLocalEvent<TriggerOnTimedCollideComponent, EndCollideEvent>(OnTimerEndCollide);
        SubscribeLocalEvent<TriggerOnTimedCollideComponent, ComponentRemove>(OnComponentRemove);
    }

    private void OnTimerCollide(EntityUid uid, TriggerOnTimedCollideComponent component, ref StartCollideEvent args)
    {
        //Ensures the entity trigger will have an active component
        EnsureComp<ActiveTriggerOnTimedCollideComponent>(uid);
        var otherUID = args.OtherEntity;
        if (component.Colliding.ContainsKey(otherUID))
            return;
        component.Colliding.Add(otherUID, 0);
    }

    private void OnTimerEndCollide(EntityUid uid, TriggerOnTimedCollideComponent component, ref EndCollideEvent args)
    {
        var otherUID = args.OtherEntity;
        component.Colliding.Remove(otherUID);

        if (component.Colliding.Count == 0 && HasComp<ActiveTriggerOnTimedCollideComponent>(uid))
            RemComp<ActiveTriggerOnTimedCollideComponent>(uid);
    }

    private void OnComponentRemove(EntityUid uid, TriggerOnTimedCollideComponent component, ComponentRemove args)
    {
        if (HasComp<ActiveTriggerOnTimedCollideComponent>(uid))
            RemComp<ActiveTriggerOnTimedCollideComponent>(uid);
    }

    private void UpdateTimedCollide(float frameTime)
    {
        foreach (var (activeTrigger, triggerOnTimedCollide) in EntityQuery<ActiveTriggerOnTimedCollideComponent, TriggerOnTimedCollideComponent>())
        {
            foreach (var (collidingEntity, collidingTimer) in triggerOnTimedCollide.Colliding)
            {
                triggerOnTimedCollide.Colliding[collidingEntity] += frameTime;
                if (collidingTimer > triggerOnTimedCollide.Threshold)
                {
                    RaiseLocalEvent(activeTrigger.Owner, new TriggerEvent(activeTrigger.Owner, collidingEntity), true);
                    triggerOnTimedCollide.Colliding[collidingEntity] -= triggerOnTimedCollide.Threshold;
                }
            }
        }
    }
}
