// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Kai5
// SPDX-FileCopyrightText: 2025 Solstice
// SPDX-FileCopyrightText: 2025 SolsticeOfTheWinter
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Goobstation.CheatDeath;
using Content.Shared._Goobstation.Devil.Condemned;
using Content.Shared.Mobs;

namespace Content.Server._Goobstation.Devil.Condemned;
public sealed partial class CondemnedSystem
{
    public void InitializeOnDeath()
    {
        SubscribeLocalEvent<CondemnedComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(EntityUid uid, CondemnedComponent comp, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead
            || comp.SoulOwnedNotDevil
            || !comp.CondemnOnDeath)
            return;

        if (TryComp<CheatDeathComponent>(uid, out var cheatDeath)
            && cheatDeath.ReviveAmount > 0 || cheatDeath is { InfiniteRevives: true })
            return;

        StartCondemnation(uid, behavior: CondemnedBehavior.Delete);
    }
}
