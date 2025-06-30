// SPDX-FileCopyrightText: 2024 Aviu00 <93730715+Aviu00@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Weapons.Ranged.Systems;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;

namespace Content.Shared._Goobstation.Weapons.Wielding;

public sealed class UnwieldOnShootSystem : EntitySystem
{
    [Dependency] private readonly WieldableSystem _wieldable = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<UnwieldOnShootComponent, GunShotEvent>(OnShoot);
    }

    private void OnShoot(EntityUid uid, UnwieldOnShootComponent component, ref GunShotEvent args)
    {
        if (TryComp(uid, out WieldableComponent? wieldable))
            _wieldable.TryUnwield(uid, wieldable, args.User);
    }
}
