// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.NPC.Components;

/// <summary>
/// Added to NPCs whenever they're in melee combat so they can be handled by the dedicated system.
/// </summary>
[RegisterComponent]
public sealed partial class NPCMeleeCombatComponent : Component
{
    /// <summary>
    /// If the target is moving what is the chance for this NPC to miss.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float MissChance;

    [ViewVariables]
    public EntityUid Target;

    [ViewVariables]
    public CombatStatus Status = CombatStatus.Normal;

    /// <summary>
    ///     Lava edit - how much seconds does it take for a mob to begin attacking once in range.
    ///     This is to prevent instant attacks and give more time to dodge.
    /// </summary>
    // Lavaland Change Start
    [ViewVariables] public float ChargeupDelay = 1f;
    [ViewVariables] public float ChargeupTimer = 0f;
    // Lavaland Change end
}

public enum CombatStatus : byte
{
    /// <summary>
    /// The target isn't in LOS anymore.
    /// </summary>
    NotInSight,

    /// <summary>
    /// Due to some generic reason we are unable to attack the target.
    /// </summary>
    Unspecified,

    /// <summary>
    /// Set if we can't reach the target for whatever reason.
    /// </summary>
    TargetUnreachable,

    /// <summary>
    /// If the target is outside of our melee range.
    /// </summary>
    TargetOutOfRange,

    /// <summary>
    /// Set if the weapon we were assigned is no longer valid.
    /// </summary>
    NoWeapon,

    /// <summary>
    /// No dramas.
    /// </summary>
    Normal,
}
