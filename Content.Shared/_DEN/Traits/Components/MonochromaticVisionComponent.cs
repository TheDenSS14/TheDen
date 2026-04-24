// SPDX-FileCopyrightText: 2026 Dirius
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Traits.Components;

/// <summary>
/// Causes someone to see the world in monochromatic view.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MonochromaticVisionComponent : Component;