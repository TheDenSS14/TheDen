// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Physics;

namespace Content.Shared.Weapons.Ranged.Events;

/// <summary>
///     Raised after an entity fires a hitscan weapon, but before the list is truncated to the first target. Necessary for Entities that need to prevent friendly fire
/// </summary>
[ByRefEvent]
public struct HitScanAfterRayCastEvent
{
    public List<RayCastResults>? RayCastResults;

    public HitScanAfterRayCastEvent(List<RayCastResults>? rayCastResults)
    {
        RayCastResults = rayCastResults;
    }
}
