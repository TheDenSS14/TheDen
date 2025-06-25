// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos;
using Content.Shared.Clothing.Components;
using Content.Shared.Inventory;

namespace Content.Shared.Clothing.EntitySystems;

/// <summary>
/// Handles reducing fire damage when wearing clothing with <see cref="FireProtectionComponent"/>.
/// </summary>
public sealed class FireProtectionSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FireProtectionComponent, InventoryRelayedEvent<GetFireProtectionEvent>>(OnGetProtection);
    }

    private void OnGetProtection(Entity<FireProtectionComponent> ent, ref InventoryRelayedEvent<GetFireProtectionEvent> args)
    {
        args.Args.Reduce(ent.Comp.Reduction);
    }
}
