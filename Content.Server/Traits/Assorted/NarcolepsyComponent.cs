// SPDX-FileCopyrightText: 2022 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;

namespace Content.Server.Traits.Assorted;

/// <summary>
/// This is used for the narcolepsy trait.
/// </summary>
[RegisterComponent, Access(typeof(NarcolepsySystem))]
public sealed partial class NarcolepsyComponent : Component
{
    /// <summary>
    /// The random time between incidents, (min, max).
    /// </summary>
    [DataField("timeBetweenIncidents", required: true)]
    public Vector2 TimeBetweenIncidents { get; private set; }

    /// <summary>
    /// The duration of incidents, (min, max).
    /// </summary>
    [DataField("durationOfIncident", required: true)]
    public Vector2 DurationOfIncident { get; private set; }

    [DataField]
    public float NextIncidentTime;

    /// <summary>
    ///     Locales for popups shown when the entity is about to fall asleep/is waking up.
    ///     They are fetched in the format of "(base)-(random number between 1 and count)", e.g. "narcolepsy-warning-popup-3".
    /// </summary>
    [DataField]
    public string WarningLocaleBase = "narcolepsy-warning-popup", WakeupLocaleBase = "narcolepsy-wakeup-popup";

    [DataField]
    public int WarningLocaleCount = 5, WakeupLocaleCount = 3;

    [DataField]
    public float TimeBeforeWarning = 20f, WarningChancePerSecond = 0.25f;

    public float LastWarningRollTime = float.MaxValue;
}
