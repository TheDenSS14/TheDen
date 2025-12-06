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
using MathNet.Numerics;


namespace Content.Shared.Psionics.Glimmer;


/// <summary>
/// This handles setting / reading the value of glimmer.
/// </summary>
public sealed class GlimmerSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private double _glimmer = 0;

    /// <summary>
    ///     GlimmerInput represents the system-facing value of the station's glimmer.
    /// </summary>
    public double Glimmer
    {
        get => _glimmer;
        set => SetGlimmerInternal(_enabled ? value : 0);
    }

    /// <summary>
    ///     This returns a string that returns a more display-friendly glimmer input.
    /// </summary>
    public string GlimmerString()
    {
        var displayable = Math.Round(Glimmer, 2);
        return displayable.ToString("F1");
    }

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
        Glimmer = 0;
    }

    /// <summary>
    ///     Return an abstracted range of a glimmer count. This is a legacy system used to support the Prober,
    ///     and is the lowest form of abstracted glimmer. It's meant more for sprite states than math.
    /// </summary>
    /// <param name="glimmer">What glimmer count to check. Uses the current glimmer by default.</param>
    public GlimmerTier GetGlimmerTier(double? glimmer = null)
    {
        if (glimmer == null)
            glimmer = Glimmer;

        return glimmer switch
        {
            < 50 => GlimmerTier.Minimal,
            < 250 => GlimmerTier.Low,
            < 500 => GlimmerTier.Moderate,
            < 750 => GlimmerTier.High,
            < 900 => GlimmerTier.Dangerous,
            _ => GlimmerTier.Critical,
        };
    }

    private void SetGlimmerInternal(double set)
    {
        if (!_enabled || set < 0)
            return;

        var newGlimmer = Math.Clamp(set, 0, 999.999);
        var ev = new GlimmerChangedEvent(_glimmer, newGlimmer);
        _glimmer = newGlimmer;

        RaiseLocalEvent(ev);
    }

    /// <summary>
    ///     Returns the GlimmerEnabled CVar, useful for niche early exits in systems that otherwise don't have any calls to CVars.
    /// </summary>
    public bool GetGlimmerEnabled()
    {
        return _enabled;
    }
}

public sealed class GlimmerChangedEvent(double oldGlimmer, double newGlimmer) : EntityEventArgs;

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
