// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Smoking.Components;

namespace Content.Shared.Smoking.Systems;

public abstract class SharedMatchstickSystem : EntitySystem
{
    public virtual bool SetState(Entity<MatchstickComponent> ent, SmokableState state)
    {
        if (ent.Comp.CurrentState == state)
            return false;

        ent.Comp.CurrentState = state;
        Dirty(ent);
        return true;
    }
}
