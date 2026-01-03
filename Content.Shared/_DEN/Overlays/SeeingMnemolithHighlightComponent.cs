// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Robust.Shared.GameStates;

namespace Content.Shared.Drugs;

[RegisterComponent, NetworkedComponent]
public sealed partial class SeeingMnemolithHighlightComponent : SwitchableOverlayComponent
{
    public override string? ToggleAction { get; set; } = "ToggleMnemolithHighlight";

    public override Color Color { get; set; } = Color.FromHex("#7A42F8");

    [DataField]
    public override float PulseTime { get; set; } = 2f;
}

public sealed partial class ToggleMnemolithHighlightEvent : InstantActionEvent;
