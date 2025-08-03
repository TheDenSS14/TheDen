// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Fishing.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FishingLureComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid FishingRod;

    [DataField, AutoNetworkedField]
    public EntityUid? AttachedEntity;

    [ViewVariables, AutoNetworkedField]
    public TimeSpan NextUpdate;

    [DataField]
    public float UpdateInterval = 1f;
}
