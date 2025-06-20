// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.WhiteDream.BloodCult.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class PylonComponent : Component
{
    [DataField]
    public bool IsActive = true;

    [DataField]
    public float HealingDelay = 20;

    [DataField]
    public float HealingAuraRange = 5;

    [DataField]
    public float CorruptionRadius = 5;

    /// <summary>
    ///     Length of the cooldown in between tile corruptions.
    /// </summary>
    [DataField]
    public float CorruptionCooldown = 5;

    /// <summary>
    ///     Length of the cooldown in between healinng.
    /// </summary>
    [DataField]
    public float HealingCooldown = 20;

    [DataField]
    public string CultTile = "CultFloor";

    [DataField]
    public EntProtoId TileCorruptEffect = "CultTileSpawnEffect";

    [DataField]
    public SoundSpecifier BurnHandSound = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

    [DataField]
    public SoundSpecifier CorruptTileSound = new SoundPathSpecifier("/Audio/_White/BloodCult/curse.ogg");

    [DataField]
    public DamageSpecifier Healing = new();

    [DataField]
    public DamageSpecifier DamageOnInteract = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public float CorruptionAccumulator = 0;

    [ViewVariables(VVAccess.ReadOnly)]
    public float HealingAccumulator = 0;
}
