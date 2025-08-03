// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Fishing.Events;

public sealed partial class ThrowFishingLureActionEvent : WorldTargetActionEvent;

public sealed partial class PullFishingLureActionEvent : InstantActionEvent;

[Serializable, NetSerializable]
public sealed class ActiveFishingSpotComponentState : ComponentState
{
    public readonly float FishDifficulty;
    public bool IsActive;
    public TimeSpan? FishingStartTime;
    public NetEntity? AttachedFishingLure;

    public ActiveFishingSpotComponentState(float fishDifficulty, bool isActive, TimeSpan? fishingStartTime, NetEntity? attachedFishingLure)
    {
        FishDifficulty = fishDifficulty;
        IsActive = isActive;
        FishingStartTime = fishingStartTime;
        AttachedFishingLure = attachedFishingLure;
    }
}
