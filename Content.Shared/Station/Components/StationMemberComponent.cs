// SPDX-FileCopyrightText: 2022 Moony
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2024 Debug
// SPDX-FileCopyrightText: 2025 Eightballll
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Station.Components;

/// <summary>
/// Indicates that a grid is a member of the given station.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class StationMemberComponent : Component
{
    /// <summary>
    /// Station that this grid is a part of.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid Station = EntityUid.Invalid;
}
