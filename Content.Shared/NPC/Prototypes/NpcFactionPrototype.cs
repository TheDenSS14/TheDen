// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 stellar-novas <stellar_novas@riseup.net>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.NPC.Prototypes;

/// <summary>
/// Contains data about this faction's relations with other factions.
/// </summary>
[Prototype("npcFaction")]
public sealed partial class NpcFactionPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public List<ProtoId<NpcFactionPrototype>> Friendly = new();

    [DataField]
    public List<ProtoId<NpcFactionPrototype>> Hostile = new();
}

/// <summary>
/// Cached data for the faction prototype. Is modified at runtime, whereas the prototype is not.
/// </summary>
public record struct FactionData
{
    [ViewVariables]
    public bool IsHostileToSelf;

    [ViewVariables]
    public HashSet<ProtoId<NpcFactionPrototype>> Friendly;

    [ViewVariables]
    public HashSet<ProtoId<NpcFactionPrototype>> Hostile;
}
