// SPDX-FileCopyrightText: 2025 Eagle-0 <114363363+Eagle-0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Stunnable;

[RegisterComponent, NetworkedComponent]
public sealed partial class StamcritResistComponent : Component
{
    /// <summary>
    ///     If stamina damage reaches (damage * multiplier), then the entity will enter stamina crit.
    /// </summary>
    [DataField] public float Multiplier = 2f;
}