// SPDX-FileCopyrightText: 2025 Aikakakah <145503852+Aikakakah@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Humanoid;


namespace Content.Server.FloofStation.ModifyUndies;


/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ModifyUndiesComponent : Component
{
    /// <summary>
    ///     The bodypart target enums for the undies.
    /// </summary>
    public List<HumanoidVisualLayers> BodyPartTargets =
    [
        HumanoidVisualLayers.Underwear,
        HumanoidVisualLayers.Undershirt
    ];
}


