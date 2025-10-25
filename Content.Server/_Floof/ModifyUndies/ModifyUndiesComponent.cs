// SPDX-FileCopyrightText: 2025 Aikakakah
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;


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

    /// <summary>
    /// DEN: Allow removing based on the category too.
    /// </summary>
    public HashSet<MarkingCategories> CategoryTargets =
    [
        MarkingCategories.Undershirt,
        MarkingCategories.Underwear,
    ];
}
