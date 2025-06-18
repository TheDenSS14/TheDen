// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Systems;
using Content.Server.Body.Components;
using Content.Shared.Damage;

namespace Content.Server.Traits.Assorted;

public sealed class HemophiliaSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HemophiliaComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<HemophiliaComponent, DamageModifyEvent>(OnDamageModify);
    }

    private void OnStartup(EntityUid uid, HemophiliaComponent component, ComponentStartup args)
    {
        if (!TryComp<BloodstreamComponent>(uid, out var bloodstream))
            return;

        bloodstream.BleedReductionAmount *= component.BleedReductionModifier;
    }

    private void OnDamageModify(EntityUid uid, HemophiliaComponent component, DamageModifyEvent args)
    {
        args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, component.DamageModifiers);
    }
}
