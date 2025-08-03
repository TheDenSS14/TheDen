// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Ghost;

/// <summary>
/// Marker component to identify "ghostly" entities.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SpectralComponent : Component { }
