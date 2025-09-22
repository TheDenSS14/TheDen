// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Armor;
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

        SubscribeLocalEvent<FireProtectionComponent, ArmorExamineEvent>(OnArmorExamine);
        SubscribeLocalEvent<FireProtectionComponent, InventoryRelayedEvent<GetFireProtectionEvent>>(OnGetProtection);
    }

    private void OnGetProtection(Entity<FireProtectionComponent> ent, ref InventoryRelayedEvent<GetFireProtectionEvent> args)
    {
        args.Args.Reduce(ent.Comp.Reduction);
    }
    private void OnArmorExamine(EntityUid uid, FireProtectionComponent component, ref ArmorExamineEvent args)
    {
        var value = MathF.Round(component.Reduction * 100, 1);

        args.Msg.PushNewline();
        if (component.ProtectContents)
        {
            args.Msg.AddMarkupOrThrow(Loc.GetString("fire-resistance-contents-coefficient-value"));
        }
        else
        {
            args.Msg.AddMarkupOrThrow(Loc.GetString("fire-resistance-coefficient-value", ("value", value)));
        }
    }
}
