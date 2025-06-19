// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 stellar-novas <stellar_novas@riseup.net>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.NPC.Events;

/// <summary>
/// Raised from client to server to notify a faction was added to an NPC.
/// </summary>
[Serializable, NetSerializable]
public sealed class NpcFactionAddedEvent : EntityEventArgs
{
    public string FactionID;

    public NpcFactionAddedEvent(string factionId) => FactionID = factionId;
}

/// <summary>
/// Raised from client to server to notify a faction was removed from an NPC.
/// </summary>
[Serializable, NetSerializable]
public sealed class NpcFactionRemovedEvent : EntityEventArgs
{
    public string FactionID;

    public NpcFactionRemovedEvent(string factionId) => FactionID = factionId;
}
