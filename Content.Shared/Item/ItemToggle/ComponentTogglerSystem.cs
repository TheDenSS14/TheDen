// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Item.ItemToggle;

/// <summary>
/// Handles <see cref="ComponentTogglerComponent"/> component manipulation.
/// </summary>
public sealed class ComponentTogglerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ComponentTogglerComponent, ItemToggledEvent>(OnToggled);
    }

    private void OnToggled(Entity<ComponentTogglerComponent> ent, ref ItemToggledEvent args)
    {
        ToggleComponent(ent, args.Activated);
    }

    // Goobstation - Make this system more flexible
    public void ToggleComponent(EntityUid uid, bool activate)
    {
        if (!TryComp<ComponentTogglerComponent>(uid, out var component))
            return;

        var target = component.Parent ? Transform(uid).ParentUid : uid;

        if (activate)
            EntityManager.AddComponents(target, component.Components);
        else
            EntityManager.RemoveComponents(target, component.RemoveComponents ?? component.Components);
    }
}
