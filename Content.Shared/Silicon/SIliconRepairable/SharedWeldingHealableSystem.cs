// SPDX-FileCopyrightText: 2024 Timemaster99
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicon.WeldingHealing;

public abstract partial class SharedWeldingHealableSystem : EntitySystem
{
}

[Serializable, NetSerializable]
public sealed partial class SiliconRepairFinishedEvent : SimpleDoAfterEvent
{
    public float Delay;
}

/// <summary>
///     Fired when an entity repairs a silicon.
/// </summary>
public sealed partial class RepairedSiliconEvent : EntityEventArgs
{
    public bool Finished;
    public EntityUid? Target;
}
