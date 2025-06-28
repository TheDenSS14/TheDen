// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Cami <147159915+Camdot@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;
using Robust.Shared.Configuration;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;

namespace Content.Shared.Psionics.Glimmer;


/// <summary>
/// This handles setting / reading the value of glimmer.
/// </summary>
public sealed class GlimmerSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private double _glimmerInput = 0;

    /// <summary>
    ///     GlimmerInput represents the system-facing value of the station's glimmer, and is given by f(y) for this graph: https://www.desmos.com/calculator/posutiq38e
    ///     Where x = GlimmerOutput and y = GlimmerInput
    /// </summary>
    /// <remarks>
    ///     This is private set for a good reason, if you're looking to change it, do so via DeltaGlimmerInput or SetGlimmerInput
    /// </remarks>
    public double GlimmerInput
    {
        get { return _glimmerInput; }
        private set { _glimmerInput = _enabled ? Math.Max(value, 0) : 0; }
    }

    /// <summary>
    ///     This returns a string that returns a more display-friendly glimmer input.
    ///     For example, 502.03837847 will become 502.03.
    /// </summary>
    public string GlimmerInputString => _glimmerInput.ToString("#.##");

    private double _glimmerOutput = 0;

    /// <summary>
    ///     This constant is equal to the intersection of the Glimmer Equation(https://www.desmos.com/calculator/posutiq38e) and the line Y = X.
    /// </summary>
    public const double GlimmerEquilibrium = 502.941;


    /// <summary>
    ///     Glimmer Output represents the player-facing value of the station's glimmer, and is given by f(x) for this graph: https://www.desmos.com/calculator/posutiq38e
    ///     Where x = GlimmerInput and y = GlimmerOutput
    /// </summary>
    /// <remarks>
    ///     This is private set for a good reason, if you're looking to change it, do so via DeltaGlimmerOutput or SetGlimmerOutput
    /// </remarks>
    public double GlimmerOutput
    {
        get { return _glimmerOutput; }
        private set { _glimmerOutput = _enabled ? Math.Clamp(value, 0, 999.999) : 0; }
    }

    /// <summary>
    ///     This returns a string that returns a more display-friendly glimmer output.
    ///     For example, 502.03837847 will become 502.03.
    /// </summary>
    public string GlimmerOutputString => _glimmerOutput.ToString("#.##");

    private bool _enabled;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(Reset);
        _enabled = _cfg.GetCVar(CCVars.GlimmerEnabled);
        _cfg.OnValueChanged(CCVars.GlimmerEnabled, value => _enabled = value, true);
    }

    private void Reset(RoundRestartCleanupEvent args)
    {
        GlimmerInput = 0;
        GlimmerOutput = 0;
    }

    /// <summary>
    ///     Return an abstracted range of a glimmer count. This is a legacy system used to support the Prober,
    ///     and is the lowest form of abstracted glimmer. It's meant more for sprite states than math.
    /// </summary>
    /// <param name="glimmer">What glimmer count to check. Uses the current glimmer by default.</param>
    public GlimmerTier GetGlimmerTier(double? glimmer = null)
    {
        if (glimmer == null)
            glimmer = GlimmerOutput;

        return glimmer switch
        {
            < 50 => GlimmerTier.Minimal,
            < 400 => GlimmerTier.Low,
            < 600 => GlimmerTier.Moderate,
            < 700 => GlimmerTier.High,
            < 900 => GlimmerTier.Dangerous,
            _ => GlimmerTier.Critical,
        };
    }
    
    /// <summary>
    ///     Returns a 0 through 10 range of glimmer. Do not divide by this for any reason.
    /// </summary>
    /// <returns></returns>
    public int GetGlimmerOutputInteger()
    {
        if (!_enabled)
            return 1;
        else return (int) Math.Round(GlimmerOutput / 1000);
    }

    /// <summary>
    ///     This is the public facing function for modifying Glimmer based on the log scale. Simply add or subtract to this with any nonzero number
    ///     Go through this if you want glimmer to be modified faster if its below 502.941f, and slower if above said equilibrium
    /// </summary>
    /// <param name="delta"></param>
    public void DeltaGlimmerInput(double delta)
    {
        if (!_enabled || delta == 0)
            return;

        GlimmerInput = Math.Max(GlimmerInput + delta, 0);
        GlimmerOutput = Math.Clamp(2000 / (1 + Math.Pow(Math.E, -0.0022 * GlimmerInput)) - 1000, 0, 999.999);
    }

    /// <summary>
    ///     This is the public facing function for modifying Glimmer based on a linear scale. Simply add or subtract to this with any nonzero number.
    ///     This is primarily intended for load bearing systems such as Probers and Drainers, and should not be called by most things by design.
    /// </summary>
    /// <param name="delta"></param>
    public void DeltaGlimmerOutput(double delta)
    {
        if (!_enabled || delta == 0)
            return;

        GlimmerOutput = Math.Clamp(GlimmerOutput + delta, 0, 999.999);
        GlimmerInput = Math.Max(Math.Log((GlimmerOutput + 1000) / (1000 - GlimmerOutput)) / 0.0022, 0);
    }

    /// <summary>
    ///     This directly sets the Player-Facing side of Glimmer to a given value, and is not intended to be called by anything other than admin commands.
    ///     This is clamped between 0 and 999.999f
    /// </summary>
    /// <param name="set"></param>
    public void SetGlimmerOutput(float set)
    {
        if (!_enabled || set == 0)
            return;

        GlimmerOutput = Math.Clamp(set, 0, 999.999);
        GlimmerInput = Math.Log((GlimmerOutput + 1000) / (1000 - GlimmerOutput)) / 0.0022;
    }

    /// <summary>
    ///     This directly sets the code-facing side of Glimmer to a given value, and is not intended to be called by anything other than admin commands.
    ///     This accepts any positive float input.
    /// </summary>
    /// <param name="set"></param>
    public void SetGlimmerInput(float set)
    {
        if (!_enabled || set < 0)
            return;

        GlimmerInput = Math.Max(set, 0);
        GlimmerOutput = Math.Clamp(2000 / (1 + Math.Pow(Math.E, -.0022 * set)) - 1000, 0, 999.999);
    }

    /// <summary>
    ///     Outputs the ratio between actual glimmer and glimmer equilibrium(The intersection of the Glimmer Equation and the line y = x).
    ///     This will return 0.01 if glimmer is 0, and 1 if glimmer is disabled.
    /// </summary>
    public double GetGlimmerEquilibriumRatio()
    {
        if (!_enabled)
            return 1;
        else if (GlimmerOutput == 0)
            return 0.01;
        else return GlimmerOutput / GlimmerEquilibrium;
    }

    /// <summary>
    ///     Returns the GlimmerEnabled CVar, useful for niche early exits in systems that otherwise don't have any calls to CVars.
    /// </summary>
    public bool GetGlimmerEnabled()
    {
        return _enabled;
    }
}

[Serializable, NetSerializable]
public enum GlimmerTier : byte
{
    Minimal,
    Low,
    Moderate,
    High,
    Dangerous,
    Critical,
}
