// SPDX-FileCopyrightText: 2025 Winkarst-cpu
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Medical;

namespace Content.Server.Destructible.Thresholds.Behaviors;

[DataDefinition]
public sealed partial class VomitBehavior : IThresholdBehavior
{
    public void Execute(EntityUid uid, DestructibleSystem system, EntityUid? cause = null)
    {
        system.EntityManager.System<VomitSystem>().Vomit(uid);
    }
}
