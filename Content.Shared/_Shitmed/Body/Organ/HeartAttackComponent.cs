// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Shitmed.Body.Organ;

// I wanted to name this SheerHeartAttackComponent :(
[RegisterComponent]
public sealed partial class HeartAttackComponent : Component
{

    /// <summary>
    ///     Movement speed modifier for walking.
    /// </summary>
    [DataField]
    public float WalkSpeed = 1f;

    /// <summary>
    ///     Movement speed modifier for sprinting.
    /// </summary>
    [DataField]
    public float SprintSpeed = 1f;
}