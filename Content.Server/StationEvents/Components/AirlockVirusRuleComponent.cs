// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent]
public sealed partial class AirlockVirusRuleComponent : Component
{
    /// <summary>
    ///     The minimum amount of time in seconds before each infected door is self-emagged.
    /// </summary>
    [DataField]
    public int MinimumTimeToEmag = 30;

    /// <summary>
    ///     The maximum amount of time in seconds before each infected door is self-emagged.
    /// </summary>
    [DataField]
    public int MaximumTimeToEmag = 120;
}
