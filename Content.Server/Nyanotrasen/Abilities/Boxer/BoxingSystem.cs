// SPDX-FileCopyrightText: 2023 Fluffiest Floofers <thebluewulf@gmail.com>
// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage.Events;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Containers;

namespace Content.Server.Abilities.Boxer;

public sealed partial class BoxingSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BoxerComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<BoxerComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<BoxingGlovesComponent, TakeStaminaDamageEvent>(OnStamHit);
    }

    private void OnInit(EntityUid uid, BoxerComponent component, ComponentInit args)
    {
        if (TryComp<MeleeWeaponComponent>(uid, out var meleeComp))
            meleeComp.Range *= component.RangeBonus;
    }
    private void OnMeleeHit(EntityUid uid, BoxerComponent component, MeleeHitEvent args)
    {
        args.ModifiersList.Add(component.UnarmedModifiers);
    }

    private void OnStamHit(EntityUid uid, BoxingGlovesComponent component, TakeStaminaDamageEvent args)
    {
        if (!_containerSystem.TryGetContainingContainer(uid, out var equipee))
            return;

        if (TryComp<BoxerComponent>(equipee.Owner, out var boxer))
            args.Multiplier *= boxer.BoxingGlovesModifier;
    }
}
