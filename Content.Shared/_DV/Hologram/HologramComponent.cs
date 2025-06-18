// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._DV.Hologram;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedHologramSystem))]
public sealed partial class HologramComponent : Component
{
    /// <summary>
    /// To save the state of whatever the component was added to, in case it occludes when the component is added.
    /// </summary>
    [DataField("occludes")]
    public bool Occludes = false;

    /// <summary>
    /// Do we stop disarms or not?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("preventDisarm")]
    public bool PreventDisarm = false;
}
