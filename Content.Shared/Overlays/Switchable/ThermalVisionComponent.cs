// SPDX-FileCopyrightText: 2025 Spatison <137375981+Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Robust.Shared.GameStates;

namespace Content.Shared.Overlays.Switchable;

[RegisterComponent, NetworkedComponent]
public sealed partial class ThermalVisionComponent : SwitchableOverlayComponent
{
    public override string? ToggleAction { get; set; } = "ToggleThermalVision";

    public override Color Color { get; set; } = Color.FromHex("#F84742");

    [DataField]
    public override float PulseTime { get; set; } = 2f;

    [DataField]
    public float LightRadius = 5f;
}

public sealed partial class ToggleThermalVisionEvent : InstantActionEvent;
