// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NPC.Components;


namespace Content.Shared.NPC.Systems;

public sealed partial class NpcFactionSystem
{
    public void InitializeCore()
    {
        SubscribeLocalEvent<NpcFactionMemberComponent, GetNearbyHostilesEvent>(OnGetNearbyHostiles);
    }

    public bool ContainsFaction(EntityUid uid, string faction, NpcFactionMemberComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return false;

        return component.Factions.Contains(faction);
    }

    public void AddFriendlyEntity(EntityUid uid, EntityUid fEntity, NpcFactionMemberComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.ExceptionalFriendlies.Add(fEntity);
    }

    private void OnGetNearbyHostiles(EntityUid uid, NpcFactionMemberComponent component, ref GetNearbyHostilesEvent args)
    {
        args.ExceptionalFriendlies.UnionWith(component.ExceptionalFriendlies);
    }
}

/// <summary>
/// Raised on an entity when it's trying to determine which nearby entities are hostile.
/// </summary>
/// <param name="ExceptionalHostiles">Entities that will be counted as hostile regardless of faction. Overriden by friendlies.</param>
/// <param name="ExceptionalFriendlies">Entities that will be counted as friendly regardless of faction. Overrides hostiles. </param>
[ByRefEvent]
public readonly record struct GetNearbyHostilesEvent(HashSet<EntityUid> ExceptionalHostiles, HashSet<EntityUid> ExceptionalFriendlies);
