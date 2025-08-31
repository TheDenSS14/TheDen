// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chemistry.Components;
using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Hypospray.Events;

namespace Content.Server._DEN.Chemistry.EntitySystems;

public sealed class AfterInjectSystem : SharedAfterInjectSystem
{
    [Dependency] private readonly IComponentFactory _factory = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AddComponentAfterInjectionComponent,
            HyposprayDoAfterEvent>(AddComponentAfterHyposprayInject, after: [typeof(SharedHypospraySystem)]);
    }

    public void AddComponentAfterHyposprayInject(Entity<AddComponentAfterInjectionComponent> entity,
        ref HyposprayDoAfterEvent args)
    {
        if (args.Handled && args.Target != null)
            AddComponentAfterInject(entity, args.Target.Value);
    }

    public void AddComponentAfterInject(Entity<AddComponentAfterInjectionComponent> entity,
        EntityUid target)
    {
        foreach (var component in entity.Comp.ComponentsToAdd)
        {
            if (EntityManager.HasComponent(target, _factory.GetComponent(component.Key).GetType()))
                continue;

            EntityManager.AddComponent(target, component.Value);
        }
    }

}
