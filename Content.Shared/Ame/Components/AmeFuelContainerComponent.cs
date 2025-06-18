// SPDX-FileCopyrightText: 2024 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Ame.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AmeFuelContainerComponent : Component
{
    /// <summary>
    /// The amount of fuel in the container.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int FuelAmount = 1000;

    /// <summary>
    /// The maximum fuel capacity of the container.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int FuelCapacity = 1000;
}
