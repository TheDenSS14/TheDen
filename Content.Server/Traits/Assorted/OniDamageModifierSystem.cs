// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Abilities.Oni;
using Content.Shared.Damage;

namespace Content.Server.Traits.Assorted;

public sealed class OniDamageModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<OniDamageModifierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, OniDamageModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<OniComponent>(uid, out var oni))
            return;

        foreach (var (key, value) in component.MeleeModifierReplacers.Coefficients)
        {
            oni.MeleeModifiers.Coefficients[key] = value;

        }

        foreach (var (key, value) in component.MeleeModifierReplacers.FlatReduction)
        {
            oni.MeleeModifiers.FlatReduction[key] = value;

        }
    }
}
