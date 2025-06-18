// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.GameTicking.Rules;
using Content.Shared.Whitelist;
using Robust.Shared.Map;

/// <summary>
/// Stores grids created by another gamerule component.
/// With <c>AntagSelection</c>, spawners on these grids can be used for its antags.
/// </summary>
[RegisterComponent, Access(typeof(RuleGridsSystem))]
public sealed partial class RuleGridsComponent : Component
{
    /// <summary>
    /// The map that was loaded.
    /// </summary>
    [DataField]
    public MapId? Map;

    /// <summary>
    /// The grid entities that have been loaded.
    /// </summary>
    [DataField]
    public List<EntityUid> MapGrids = new();

    /// <summary>
    /// Whitelist for a spawner to be considered for an antag.
    /// All spawners must have <c>SpawnPointComponent</c> regardless to be found.
    /// </summary>
    [DataField]
    public EntityWhitelist? SpawnerWhitelist;
}
