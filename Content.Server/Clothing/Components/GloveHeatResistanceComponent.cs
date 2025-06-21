// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Clothing.Components;

/// <summary>
///     TODO this needs removed somehow.
///     Handles 'heat resistance' for gloves touching bulbs and that's it, ick.
/// </summary>
[RegisterComponent]
public sealed partial class GloveHeatResistanceComponent : Component
{
    [DataField("heatResistance")]
    public int HeatResistance = 323;
}
