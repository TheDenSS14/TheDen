// SPDX-FileCopyrightText: 2025 BloodfiendishOperator <141253729+Diggy0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Goobstation.Clothing.Components;
using Content.Shared.Damage;
using Robust.Shared.Timing;

namespace Content.Shared._Goobstation.Clothing.Systems;

public sealed class DamageOverTimeSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly DamageableSystem _damageSys = default!;

    public override void Update(float frameTime)
    {
        var currentTime = _timing.CurTime;
        var query = EntityQueryEnumerator<DamageOverTimeComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (currentTime < component.NextTickTime)
                continue;
            component.NextTickTime = currentTime + component.Interval;
            _damageSys.TryChangeDamage(uid, component.Damage, ignoreResistances: component.IgnoreResistances);
        }
    }
}
