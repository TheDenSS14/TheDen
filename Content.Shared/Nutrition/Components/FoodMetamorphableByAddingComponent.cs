// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.Components;

/// <summary>
/// Attempts to metamorphose a modular food when a new ingredient is added.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedFoodSequenceSystem))]
public sealed partial class FoodMetamorphableByAddingComponent : Component
{
    /// <summary>
    /// if true, the metamorphosis will only be attempted when the sequence ends, not when each element is added.
    /// </summary>
    [DataField]
    public bool OnlyFinal = true;
}
