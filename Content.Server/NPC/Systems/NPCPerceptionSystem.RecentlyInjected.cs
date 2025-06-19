// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.NPC.Components;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCPerceptionSystem
{
    /// <summary>
    /// Tracks targets recently injected by medibots.
    /// </summary>
    /// <param name="frameTime"></param>
    private void UpdateRecentlyInjected(float frameTime)
    {
        var query = EntityQueryEnumerator<NPCRecentlyInjectedComponent>();
        while (query.MoveNext(out var uid, out var entity))
        {
            entity.Accumulator += frameTime;
            if (entity.Accumulator < entity.RemoveTime.TotalSeconds)
                continue;
            entity.Accumulator = 0;

            RemComp<NPCRecentlyInjectedComponent>(uid);
        }
    }
}
