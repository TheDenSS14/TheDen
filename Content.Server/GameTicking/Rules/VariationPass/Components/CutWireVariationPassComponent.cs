// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Whitelist;

namespace Content.Server.GameTicking.Rules.VariationPass.Components;

/// <summary>
/// Handles cutting a random wire on random devices around the station.
/// </summary>
[RegisterComponent]
public sealed partial class CutWireVariationPassComponent : Component
{
    /// <summary>
    /// Blacklist of hackable entities that should not be chosen to
    /// have wires cut.
    /// </summary>
    [DataField]
    public EntityWhitelist Blacklist = new();

    /// <summary>
    /// Chance for an individual wire to be cut.
    /// </summary>
    [DataField]
    public float WireCutChance = 0.05f;

    /// <summary>
    /// Maximum number of wires that can be cut stationwide.
    /// </summary>
    [DataField]
    public int MaxWiresCut = 10;
}
