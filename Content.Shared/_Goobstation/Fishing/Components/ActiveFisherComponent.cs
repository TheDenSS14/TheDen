// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Fishing.Components;

/// <summary>
/// Applied to players that are pulling fish out from water
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ActiveFisherComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan? NextStruggle;

    [DataField, AutoNetworkedField]
    public float? TotalProgress;

    [DataField, AutoNetworkedField]
    public float ProgressPerUse = 0.05f;

    [DataField, AutoNetworkedField]
    public EntityUid FishingRod;
}
