// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Server.StationGoal
{
    [Serializable, Prototype("stationGoal")]
    public sealed partial class StationGoalPrototype : IPrototype
    {
        [IdDataFieldAttribute] public string ID { get; } = default!;

        public string Text => Loc.GetString($"station-goal-{ID.ToLower()}");
    }
}
