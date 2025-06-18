// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Body.Organ;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HeartComponent : Component
{
    /// <summary>
    ///     The base capacity of the heart.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Capacity;

    /// <summary>
    ///     The current capacity of the heart.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CurrentCapacity;
}
