// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.WhiteDream.BloodCult.Runes.Offering;

[RegisterComponent]
public sealed partial class CultRuneOfferingComponent : Component
{
    /// <summary>
    ///     The lookup range for offering targets
    /// </summary>
    [DataField]
    public float OfferingRange = 0.5f;

    /// <summary>
    ///     The amount of cultists require to convert a living target.
    /// </summary>
    [DataField]
    public int ConvertInvokersAmount = 2;

    /// <summary>
    ///     The amount of cultists required to sacrifice a living target.
    /// </summary>
    [DataField]
    public int AliveSacrificeInvokersAmount = 3;

    /// <summary>
    ///     The amount of charges revive rune system should recieve on sacrifice/convert.
    /// </summary>
    [DataField]
    public int ReviveChargesPerOffering = 1;

    [DataField]
    public EntProtoId SoulShardProto = "SoulShard";

    [DataField]
    public EntProtoId SoulShardGhostProto = "SoulShardGhost";

    [DataField]
    public DamageSpecifier ConvertHealing = new()
    {
        DamageDict = new()
        {
            ["Brute"] = -40,
            ["Burn"] = -40
        }
    };
}
