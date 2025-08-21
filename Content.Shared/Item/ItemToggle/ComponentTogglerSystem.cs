// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 Eris
// SPDX-FileCopyrightText: 2025 Perry Fraser
// SPDX-FileCopyrightText: 2025 sleepyyapril
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

        EntityManager.AddComponents(target, component.Components);

        if (activate)
        {
        // Begin DeltaV - allow swapping components
        if (component.DeactivatedComponents is { } deactivatedComps)
            EntityManager.RemoveComponents(target, deactivatedComps);
        // End DeltaV
        EntityManager.AddComponents(target, component.Components);
        }
        else
        {
            EntityManager.RemoveComponents(target, component.RemoveComponents ?? component.Components);
            // Begin DeltaV - allow swapping components
            if (component.DeactivatedComponents is { } reactivatedComps)
                EntityManager.AddComponents(target, reactivatedComps);
            // End DeltaV
        }
    }
}
