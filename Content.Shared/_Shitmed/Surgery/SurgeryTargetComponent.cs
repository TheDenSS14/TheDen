// SPDX-FileCopyrightText: 2024 gluesniffler
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryTargetComponent : Component
{
    [DataField]
    public bool CanOperate = true;

    [DataField]
    public bool CanOperateOn = true;
}
