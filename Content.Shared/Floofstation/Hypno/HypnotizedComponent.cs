// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Floofstation.Hypno;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HypnotizedComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? Master;
}
