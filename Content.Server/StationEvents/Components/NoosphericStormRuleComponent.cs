// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(NoosphericStormRule))]
public sealed partial class NoosphericStormRuleComponent : Component
{
    /// <summary>
    /// How many potential psionics should be awakened at most.
    /// </summary>
    [DataField("maxAwaken")]
    public int MaxAwaken = 3;

    /// <summary>
    /// </summary>
    [DataField("baseGlimmerAddMin")]
    public int BaseGlimmerAddMin = 65;

    /// <summary>
    /// </summary>
    [DataField("baseGlimmerAddMax")]
    public int BaseGlimmerAddMax = 85;

    /// <summary>
    /// Multiply the EventSeverityModifier by this to determine how much extra glimmer to add.
    /// </summary>
    [DataField("glimmerSeverityCoefficient")]
    public float GlimmerSeverityCoefficient = 0.25f;
}
