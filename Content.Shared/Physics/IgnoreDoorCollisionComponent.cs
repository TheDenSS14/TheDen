// SPDX-FileCopyrightText: 2025 SleepyScarecrow
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Physics;


/// <summary>
/// Add this to stuff that needs to be ignored by the doors, but still needs its other colliders to stay the same
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class IgnoreDoorCollisionComponent : Component;
