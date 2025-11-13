// SPDX-FileCopyrightText: 2020 nuke
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Clyybber
// SPDX-FileCopyrightText: 2021 Galactic Chimp
// SPDX-FileCopyrightText: 2021 Leon Friedrich
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Ygg01
// SPDX-FileCopyrightText: 2021 metalgearsloth
// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 themias
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 keronshb
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 Vasilis
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.FixedPoint;

namespace Content.Shared.Fluids.Components;

/// <summary>
/// Makes a solution contained in this entity spillable.
/// Spills can occur when a container with this component overflows,
/// is used to melee attack something, is equipped (see <see cref="SpillWorn"/>),
/// lands after being thrown, or has the Spill verb used.
/// </summary>
[RegisterComponent]
public sealed partial class SpillableComponent : Component
{
    [DataField("solution")]
    public string SolutionName = "puddle";

    /// <summary>
    ///     Should this item be spilled when worn as clothing?
    ///     Doesn't count for pockets or hands.
    /// </summary>
    [DataField]
    public bool SpillWorn = true;

    [DataField]
    public float? SpillDelay;

    /// <summary>
    ///     At most how much reagent can be splashed on someone at once?
    /// </summary>
    [DataField]
    public FixedPoint2 MaxMeleeSpillAmount = FixedPoint2.New(20);

    /// <summary>
    ///     Should this item be spilled when thrown?
    /// </summary>
    [DataField]
    public bool SpillWhenThrown = true;

    /// <summary>
    ///     Should this item spill AND melee attack?
    /// </summary>
    [DataField]
    public bool AlsoMeleeAttack = false;
}
