// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.Camera;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class CameraRecoilComponent : Component
{
    public Vector2 CurrentKick { get; set; }
    public float LastKickTime { get; set; }

    /// <summary>
    ///     Basically I needed a way to chain this effect for the attack lunge animation. Sorry!
    /// </summary>
    public Vector2 BaseOffset { get; set; }
}
