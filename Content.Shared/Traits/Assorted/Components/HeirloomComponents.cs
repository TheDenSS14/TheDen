// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Mood;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits.Assorted.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HeirloomHaverComponent : Component
{
    [AutoNetworkedField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid Heirloom;

    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<MoodEffectPrototype> Moodlet = "HeirloomSecure";
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HeirloomComponent : Component
{
    [AutoNetworkedField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid HOwner;
}
