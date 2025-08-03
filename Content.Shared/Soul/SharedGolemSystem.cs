// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Containers;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Shared.Soul;

public abstract class SharedGolemSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        // I can think of better ways to handle this, but they require API changes upstream.
        SubscribeLocalEvent<GunHeldByGolemComponent, AttemptShootEvent>(OnAttemptShoot);
    }

    protected void SharedOnEntInserted(EntInsertedIntoContainerMessage args)
    {
        if (HasComp<GunComponent>(args.Entity))
            AddComp<GunHeldByGolemComponent>(args.Entity);
    }

    protected void SharedOnEntRemoved(EntRemovedFromContainerMessage args)
    {
        if (HasComp<GunComponent>(args.Entity))
            RemComp<GunHeldByGolemComponent>(args.Entity);
    }

    private void OnAttemptShoot(EntityUid uid, GunHeldByGolemComponent component, ref AttemptShootEvent args)
    {
        args.Cancelled = true;
        args.Message = Loc.GetString("golem-no-using-guns-popup");
    }
}
