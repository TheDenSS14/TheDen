// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.Graphics;
using Robust.Shared.GameStates;

namespace Content.Client.Flight.Components;

[RegisterComponent]
public sealed partial class FlightVisualsComponent : Component
{
    /// <summary>
    ///     How long does the animation last
    /// </summary>
    [DataField]
    public float Speed;

    /// <summary>
    ///     How far it goes in any direction.
    /// </summary>
    [DataField]
    public float Multiplier;

    /// <summary>
    ///     How much the limbs (if there are any) rotate.
    /// </summary>
    [DataField]
    public float Offset;

    /// <summary>
    ///     Are we animating layers or the entire sprite?
    /// </summary>
    public bool AnimateLayer = false;
    public int? TargetLayer;

    [DataField]
    public string AnimationKey = "default";

    [ViewVariables(VVAccess.ReadWrite)]
    public ShaderInstance Shader = default!;


}