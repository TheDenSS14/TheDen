// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(NoosphericZapRule))]
public sealed partial class NoosphericZapRuleComponent : Component
{
    /// <summary>
    ///     How long (in seconds) should this event stun its victims.
    /// </summary>
    public float StunDuration = 5f;

    /// <summary>
    ///     How long (in seconds) should this event give its victims the Stuttering condition.
    /// </summary>
    public float StutterDuration = 10f;

    /// <summary>
    ///     When paralyzing a Psion with a reroll still available, how much should this event modify the odds of generating a power.
    /// </summary>
    public float PowerRerollMultiplier = 0.25f;
}
