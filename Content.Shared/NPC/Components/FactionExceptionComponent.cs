// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NPC.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.NPC.Components;

/// <summary>
/// Prevents an NPC from attacking ignored entities from enemy factions.
/// Can be added to if pettable, see PettableFriendComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(NpcFactionSystem))]
public sealed partial class FactionExceptionComponent : Component
{
    /// <summary>
    /// Collection of entities that this NPC will refuse to attack
    /// </summary>
    [DataField]
    public HashSet<EntityUid> Ignored = new();

    /// <summary>
    /// Collection of entities that this NPC will attack, regardless of faction.
    /// </summary>
    [DataField]
    public HashSet<EntityUid> Hostiles = new();
}
