// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <***>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Shitmed.Medical.Surgery.Tools;

public interface ISurgeryToolComponent
{
    [DataField]
    public string ToolName { get; }

    /// <summary>
    ///     Field intended for discardable or non-reusable tools.
    /// </summary>
    [DataField]
    public bool? Used { get; set; }

    /// <summary>
    ///     Multiply the step's doafter by this value.
    ///     This is per-type so you can have something that's a good scalpel but a bad retractor.
    /// </summary>
    [DataField]
    public float Speed { get; set; }
}
