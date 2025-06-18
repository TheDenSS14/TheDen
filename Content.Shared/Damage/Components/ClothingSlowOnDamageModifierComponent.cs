// SPDX-FileCopyrightText: 2024 Blu <79374236+BlueHNT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

/// <summary>
/// This is used for a clothing item that modifies the slowdown from taking damage.
/// Used for entities with <see cref="SlowOnDamageComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SlowOnDamageSystem))]
public sealed partial class ClothingSlowOnDamageModifierComponent : Component
{
    /// <summary>
    /// A coefficient modifier for the slowdown
    /// </summary>
    [DataField]
    public float Modifier = 1;
}
