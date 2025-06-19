// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.Components;

/// <summary>
/// Blocks all attempts to access solutions contained by this entity.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BlockSolutionAccessComponent : Component
{
}
