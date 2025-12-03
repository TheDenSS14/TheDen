// SPDX-FileCopyrightText: 2025 William Lemon
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Effect;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedRMCEffectSystem))]
public sealed partial class EffectAlphaAnimationComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan? SpawnedAt;

    [DataField, AutoNetworkedField]
    public TimeSpan Delay = TimeSpan.FromSeconds(1);
}
