// SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Damage.Components;
using Content.Shared.Damage;
using Robust.Shared.Player;
using Content.Shared.Weapons.Melee.Events;
using System.Linq;

namespace Content.Server.Damage.Systems;

public sealed class DamageOnHitSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    private readonly Random _random = new Random();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DamageOnHitComponent, MeleeHitEvent>(DamageSelf);
    }

    // Looks for a hit, then damages the entity an appropriate amount.
    private void DamageSelf(EntityUid uid, DamageOnHitComponent component, MeleeHitEvent args)
    {
        if (args.HitEntities.Any()) {
            _damageableSystem.TryChangeDamage(uid, component.Damage, component.IgnoreResistances,
                targetPart: component.TargetParts is not null
                            ? component.TargetParts[_random.Next(component.TargetParts.Count)]
                            : null);
        }
    }
}
