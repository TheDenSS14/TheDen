// SPDX-FileCopyrightText: 2024 Timemaster99 <57200767+Timemaster99@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicon.WeldingHealing;

public abstract partial class SharedWeldingHealableSystem : EntitySystem
{
    [Serializable, NetSerializable]
    protected sealed partial class SiliconRepairFinishedEvent : SimpleDoAfterEvent
    {
        public float Delay;
    }
}

