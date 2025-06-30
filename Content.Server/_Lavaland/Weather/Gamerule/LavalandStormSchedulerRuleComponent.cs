// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Destructible.Thresholds;

namespace Content.Server._Lavaland.Weather.Gamerule;

[RegisterComponent]
public sealed partial class LavalandStormSchedulerRuleComponent : Component
{
    /// <summary>
    ///     How long until the next check for an event runs
    /// </summary>
    [DataField] public float EventClock = 600f; // Ten minutes

    /// <summary>
    ///     How much time it takes in seconds for a lavaland storm to be raised.
    /// </summary>
    [DataField] public MinMax Delays = new(20 * 60, 40 * 60);
}
