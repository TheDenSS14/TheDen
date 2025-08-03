// SPDX-FileCopyrightText: 2024 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Item.ItemToggle.Components;

/// <summary>
///   Handles the changes to the throwing angle when the item is toggled.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ItemToggleThrowingAngleComponent : Component
{
    /// <summary>
    ///   Item's throwing spin status when activated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool? ActivatedAngularVelocity = null;

    /// <summary>
    ///   Item's angle when activated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle? ActivatedAngle = null;

    /// <summary>
    ///   Item's throwing spin status when deactivated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool? DeactivatedAngularVelocity = null;

    /// <summary>
    ///   Item's angle when deactivated.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle? DeactivatedAngle = null;

    /// <summary>
    ///   When this is true, adds the ThrowingAngle component on activation
    ///   and deletes it on deactivation.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool DeleteOnDeactivate = false;
}
