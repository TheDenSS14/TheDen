// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Physics;

/// <summary>
///     A component that allows an entity to have friction (linear and angular dampening)
///     even when not being affected by gravity.
/// </summary>
[RegisterComponent]
public sealed partial class PassiveDampeningComponent : Component
{
    [DataField]
    public bool Enabled = true;

    [DataField]
    public float LinearDampening = 0.2f;

    [DataField]
    public float AngularDampening = 0.2f;
}
