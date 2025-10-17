// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Whitelist;

/// <summary>
/// Marker component for holobarriers, used for reclaiming charges of the projector.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HolobarrierComponent : Component;
