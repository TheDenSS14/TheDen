// SPDX-FileCopyrightText: 2025 Terkala
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Zombies;

/// <summary>
/// Makes an entity immune to zombie tumor infections.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ZombieTumorImmuneComponent : Component
{
}

