// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Physics;
using Robust.Shared.Physics.Events;

namespace Content.Shared._Goobstation.Projectiles.ProjectileIgnoreTransparent;

public sealed class ProjectileIgnoreTransparentSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ProjectileIgnoreTransparentComponent, PreventCollideEvent>(OnPreventCollide);
    }

    private void OnPreventCollide(Entity<ProjectileIgnoreTransparentComponent> ent, ref PreventCollideEvent args)
    {
        if ((args.OtherFixture.CollisionLayer & (int) CollisionGroup.Opaque) == 0)
            args.Cancelled = true;
    }
}
