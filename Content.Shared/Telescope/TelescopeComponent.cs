// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Telescope;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TelescopeComponent : Component
{
    [DataField, AutoNetworkedField]
    public float Divisor = 0.1f;

    [DataField, AutoNetworkedField]
    public float LerpAmount = 0.1f;

    [ViewVariables]
    public EntityUid? LastEntity;
}
