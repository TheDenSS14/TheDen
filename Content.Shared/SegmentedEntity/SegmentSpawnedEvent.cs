// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.SegmentedEntity;

public sealed class SegmentSpawnedEvent : EntityEventArgs
{
    public EntityUid Lamia = default!;

    public SegmentSpawnedEvent(EntityUid lamia)
    {
        Lamia = lamia;
    }
}
