// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Stunnable.Components;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable.Events;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Server.Stunnable.Systems;

public sealed class KnockdownOnHitSystem : EntitySystem
{
    [Dependency] private readonly StunSystem _stun = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<KnockdownOnHitComponent, MeleeHitEvent>(OnMeleeHit);
    }

    private void OnMeleeHit(Entity<KnockdownOnHitComponent> entity, ref MeleeHitEvent args)
    {
        if (args.Direction.HasValue || !args.IsHit || !args.HitEntities.Any() || entity.Comp.Duration <= TimeSpan.Zero)
            return;

        var ev = new KnockdownOnHitAttemptEvent();
        RaiseLocalEvent(entity, ref ev);
        if (ev.Cancelled)
            return;

        foreach (var target in args.HitEntities)
        {
            if (!TryComp(target, out StatusEffectsComponent? statusEffects))
                continue;

            _stun.TryKnockdown(target,
                entity.Comp.Duration,
                entity.Comp.RefreshDuration,
                entity.Comp.DropHeldItemsBehavior,
                statusEffects);
        }
    }
}
