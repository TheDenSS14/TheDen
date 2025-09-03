// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Unrotting;
using Content.Shared.Mobs;


namespace Content.Server.Devour;

public sealed partial class DevourSystem
{
    private void OnMobStateChanged(Entity<DragonUnrottingComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Alive)
            RemComp<DragonUnrottingComponent>(ent);
    }
}
