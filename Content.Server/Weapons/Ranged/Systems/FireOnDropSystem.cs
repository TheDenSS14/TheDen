// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Throwing;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Random;

namespace Content.Server.Weapons.Ranged.Systems;

public sealed class FireOnDropSystem : EntitySystem
{
    [Dependency] private readonly SharedGunSystem _gun = default!;
    [Dependency] private readonly IRobustRandom _random = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunComponent, ThrowDoHitEvent>(HandleLand);
    }


    private void HandleLand(EntityUid uid, GunComponent component, ref ThrowDoHitEvent args)
    {
        if (_random.Prob(component.FireOnDropChance))
            _gun.AttemptShoot(uid, uid, component, Transform(uid).Coordinates.Offset(Transform(uid).LocalRotation.ToVec()));
    }
}
