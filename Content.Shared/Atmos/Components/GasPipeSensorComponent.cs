// SPDX-FileCopyrightText: 2024 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Components;

/// <summary>
/// Entities with component will be queried against for their
/// atmos monitoring data on atmos monitoring consoles
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class GasPipeSensorComponent : Component;
