// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;


namespace Content.Shared._DEN.Unrotting;


/// <summary>
/// This is used for preventing rot.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DragonUnrottingComponent : Component;
