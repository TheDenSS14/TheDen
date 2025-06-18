// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;

namespace Content.Shared.Movement.Systems;

public abstract class SharedSpriteMovementSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpriteMovementComponent, SpriteMoveEvent>(OnSpriteMoveInput);
    }

    private void OnSpriteMoveInput(Entity<SpriteMovementComponent> ent, ref SpriteMoveEvent args)
    {
        if (ent.Comp.IsMoving == args.IsMoving)
            return;

        ent.Comp.IsMoving = args.IsMoving;
        Dirty(ent);
    }
}
