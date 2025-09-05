// SPDX-FileCopyrightText: 2023 Bixkitts
// SPDX-FileCopyrightText: 2024 Debug
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 Winter
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.ContainerHeld;

[RegisterComponent, NetworkedComponent]
public sealed partial class ContainerHeldComponent: Component
{
    /// <summary>
    ///     The amount of weight needed to be in the container
    ///     in order for it to toggle it's appearance
    ///     to ToggleableVisuals.Enabled = true, and
    ///     SetHeldPrefix() to "full" instead of "empty".
    /// </summary>
    [DataField("threshold")]
    public int Threshold { get; private set; } = 1;
}
