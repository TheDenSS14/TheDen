// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Physics.Events;

namespace Content.Server.WhiteDream.BloodCult.BloodBoilProjectile;

public sealed class BloodBoilProjectileSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodBoilProjectileComponent, PreventCollideEvent>(CheckCollision);
    }

    private void CheckCollision(Entity<BloodBoilProjectileComponent> ent, ref PreventCollideEvent args)
    {
        if (args.OtherEntity != ent.Comp.Target)
            args.Cancelled = true;
    }
}
