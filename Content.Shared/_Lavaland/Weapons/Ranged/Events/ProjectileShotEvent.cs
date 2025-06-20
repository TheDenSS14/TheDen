// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Lavaland.Weapons.Ranged.Events;

/// <summary>
/// Raised on a gun when a projectile has been fired from it.
/// </summary>
public sealed class ProjectileShotEvent : EntityEventArgs
{
    public EntityUid FiredProjectile = default!;
}


