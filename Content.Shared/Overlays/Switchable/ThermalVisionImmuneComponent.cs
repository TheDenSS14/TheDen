// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._EE.Overlays.Switchable;

// imp. if an entity has this component, it will be ignored by the thermal vision overlay. this includes if it's in a container, or is itself a container.
[RegisterComponent, NetworkedComponent]
public sealed partial class ThermalVisionImmuneComponent : Component
{

}
