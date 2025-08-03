// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared;
using Content.Shared.Light.Components;
using Robust.Shared.Random;

namespace Content.Server.Light.EntitySystems;

/// <inheritdoc/>
public sealed class LightCycleSystem : SharedLightCycleSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    protected override void OnCycleMapInit(Entity<LightCycleComponent> ent, ref MapInitEvent args)
    {
        base.OnCycleMapInit(ent, ref args);

        if (ent.Comp.InitialOffset)
        {
            ent.Comp.Offset = _random.Next(ent.Comp.Duration);
            Dirty(ent);
        }
    }
}
