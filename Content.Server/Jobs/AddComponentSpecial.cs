// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 2022 Ephememory <66768086+Ephememory@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Server.Jobs
{
    [UsedImplicitly]
    public sealed partial class AddComponentSpecial : JobSpecial
    {
        [DataField, AlwaysPushInheritance]
        public ComponentRegistry Components { get; private set; } = new();

        public override void AfterEquip(EntityUid mob)
        {
            // now its a registry of components, still throws i bet.
            // TODO: This is hot garbage and probably needs an engine change to not be a POS.
            var factory = IoCManager.Resolve<IComponentFactory>();
            var entityManager = IoCManager.Resolve<IEntityManager>();
            var serializationManager = IoCManager.Resolve<ISerializationManager>();

            foreach (var (name, data) in Components)
            {
                var component = (Component) factory.GetComponent(name);
                component.Owner = mob;

                var temp = (object) component;
                serializationManager.CopyTo(data.Component, ref temp);
                entityManager.RemoveComponent(mob, temp!.GetType());
                entityManager.AddComponent(mob, (Component) temp);
            }
        }
    }
}
