// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT


namespace Content.Server.Psionics;

/// <summary>
///     Raised on an entity about to roll for a Psionic Power, after their baseline chances of success are calculated.
/// </summary>
[ByRefEvent]
public record struct OnRollPsionicsEvent(EntityUid Roller, float BaselineChance);
