// SPDX-FileCopyrightText: 2025 MajorMoth <61519600+MajorMoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Examine.Pose;

/// <summary>
/// Flavour text when this entity is examined. Can be set with an action.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedRMCSetPoseSystem))]
public sealed partial class RMCSetPoseComponent : Component
{
    [DataField, AutoNetworkedField]
    public string Pose = string.Empty;
}
