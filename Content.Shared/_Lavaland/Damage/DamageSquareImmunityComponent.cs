// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Lavaland.Damage;

/// <summary>
/// Actor having this component will not get damaged by damage squares.
/// </summary>
[RegisterComponent]
public sealed partial class DamageSquareImmunityComponent : Component
{
    [DataField]
    public TimeSpan HasImmunityUntil = TimeSpan.Zero;

    /// <summary>
    /// Setting this to true will ignore the timer and will make damage tile completely ignore an entity.
    /// </summary>
    [DataField]
    public bool IsImmune;
}
