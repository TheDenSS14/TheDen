// SPDX-FileCopyrightText: 2025 Winkarst-cpu
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Kitchen.Components;

/// <summary>
/// Used to mark entity that was butchered on the spike.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedKitchenSpikeSystem))]
public sealed partial class KitchenSpikeVictimComponent : Component;
