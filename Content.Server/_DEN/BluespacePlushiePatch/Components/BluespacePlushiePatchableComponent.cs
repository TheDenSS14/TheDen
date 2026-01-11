// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;


namespace Content.Server._DEN.BluespacePlushiePatch.Components;


/// <summary>
///     Indicates that an item can have a BluespacePlushiePatch applied.
/// </summary>
[RegisterComponent]
public sealed partial class BluespacePlushiePatchableComponent : Component
{
    [DataField]
    public bool HasPatch = false;
}
