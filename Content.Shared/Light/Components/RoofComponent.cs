// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Light.Components;

/// <summary>
/// Will draw shadows over tiles flagged as roof tiles on the attached map.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RoofComponent : Component
{
    [DataField, AutoNetworkedField]
    public Color Color = Color.Black;
}
