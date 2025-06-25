// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Light.Components;

/// <summary>
/// Applies the roof flag to this tile and deletes the entity.
/// </summary>
[RegisterComponent]
public sealed partial class SetRoofComponent : Component
{
    [DataField(required: true)]
    public bool Value;
}
