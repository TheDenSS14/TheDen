// SPDX-FileCopyrightText: 2023 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Chemistry.Components;

[RegisterComponent]
public sealed partial class SolutionHeaterComponent : Component
{
    /// <summary>
    ///     How much heat is added per second to the solution, with no upgrades.
    /// </summary>
    [DataField]
    public float BaseHeatPerSecond = 120;

    /// <summary>
    ///     How much heat is added per second to the solution, taking upgrades into account.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float HeatPerSecond;

    /// <summary>
    ///     The machine part that affects the heat multiplier.
    /// </summary>
    [DataField]
    public string MachinePartHeatMultiplier = "Capacitor";

    /// <summary>
    ///     How much each upgrade multiplies the heat by.
    /// </summary>
    [DataField]
    public float PartRatingHeatMultiplier = 1.5f;
}
