// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.WhiteDream.BloodCult.Runes.Barrier;

public sealed class CultRuneBarrierSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<CultRuneBarrierComponent, TryInvokeCultRuneEvent>(OnBarrierRuneInvoked);
    }

    private void OnBarrierRuneInvoked(Entity<CultRuneBarrierComponent> ent, ref TryInvokeCultRuneEvent args)
    {
        Spawn(ent.Comp.SpawnPrototype, Transform(ent).Coordinates);
        Del(ent);
    }
}
