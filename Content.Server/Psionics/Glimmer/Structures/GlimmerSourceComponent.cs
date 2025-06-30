// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Psionics.Glimmer;

[RegisterComponent]
/// <summary>
/// Adds to glimmer at regular intervals. We'll use it for glimmer drains too when we get there.
/// </summary>
public sealed partial class GlimmerSourceComponent : Component
{
    [DataField]
    public float Accumulator = 0f;

    [DataField]
    public bool Active = true;

    /// <summary>
    ///     The amount of glimmer to generate per second.
    /// </summary>
    [DataField]
    public double GlimmerPerSecond = 1.0;

    /// <summary>
    ///     If not null, this entity generates this value as a baseline number of research points per second, eg: Probers.
    ///     Actual glimmer research sources will scale with GlimmerEquilibriumRatio
    /// </summary>
    [DataField]
    public int? ResearchPointGeneration = null;

    /// <summary>
    ///     Controls whether this entity requires electrical power to generate research points.
    /// </summary>
    [DataField]
    public bool RequiresPower = true;

    /// <summary>
    ///     Above GlimmerEquilibrium, glimmer generation is increased exponentially, but has an offset to prevent things from spiralling out of control.
    ///     Increasing the offset will make this entity's exponential growth weaker, while decreasing it makes it stronger. Negative numbers are valid by the way :)
    /// </summary>
    [DataField]
    public int GlimmerExponentOffset = 0;
}
