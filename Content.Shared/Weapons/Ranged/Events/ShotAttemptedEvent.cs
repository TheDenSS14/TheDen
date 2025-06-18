// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Weapons.Ranged.Components;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
/// Raised on a gun when someone is attempting to shoot it.
/// Cancel this event to prevent it from shooting.
/// </summary>
[ByRefEvent]
public record struct ShotAttemptedEvent
{
    /// <summary>
    /// The user attempting to shoot the gun.
    /// </summary>
    public EntityUid User;

    /// <summary>
    /// The gun being shot.
    /// </summary>
    public Entity<GunComponent> Used;

    public bool Cancelled { get; private set; }

    /// <summary>
    /// Prevent the gun from shooting
    /// </summary>
    public void Cancel()
    {
        Cancelled = true;
    }

    /// <summary>
    /// Allow the gun to shoot again, only use if you know what you are doing
    /// </summary>
    public void Uncancel()
    {
        Cancelled = false;
    }
}
