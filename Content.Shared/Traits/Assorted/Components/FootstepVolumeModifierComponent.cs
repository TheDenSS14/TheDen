// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted.Components;

/// <summary>
///     This is used for any trait that modifies footstep volumes.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FootstepVolumeModifierComponent : Component
{
    /// <summary>
    ///     What to add to the volume of sprinting, in terms of decibels.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SprintVolumeModifier;

    /// <summary>
    ///     What to add to the volume of walking, in terms of decibels.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float WalkVolumeModifier;
}
