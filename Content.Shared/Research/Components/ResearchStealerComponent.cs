// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Research.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Research.Components;

/// <summary>
/// Component for stealing technologies from a R&D server, when gloves are enabled.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedResearchStealerSystem))]
public sealed partial class ResearchStealerComponent : Component
{
    /// <summary>
    /// Time taken to steal research from a server
    /// </summary>
    [DataField("delay"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Delay = TimeSpan.FromSeconds(20);

    /// <summary>
    /// The minimum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int MinToSteal = 4;

    /// <summary>
    /// The maximum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int MaxToSteal = 8;
}
