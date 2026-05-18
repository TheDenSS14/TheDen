// SPDX-FileCopyrightText: 2026 MajorMoth
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.Cybereye.Components;

/// <summary>
/// A bandaid fix for not being able to turn off the HUD which cybereyes provide. 
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CybereyeControlComponent : Component
{
    [DataField]
    public EntProtoId Action = "CybereyeControlAction";
    public EntityUid? ActionEntity;
}