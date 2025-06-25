// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DV.ParadoxAnomaly.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.ParadoxAnomaly.Components;

/// <summary>
/// Creates a random paradox anomaly and tranfers mind to it when taken by a player.
/// </summary>
[RegisterComponent, Access(typeof(ParadoxAnomalySystem))]
public sealed partial class ParadoxAnomalySpawnerComponent : Component
{
    /// <summary>
    /// Antag game rule to start for the paradox anomaly.
    /// </summary>
    [DataField]
    public EntProtoId Rule = "ParadoxAnomaly";
}
