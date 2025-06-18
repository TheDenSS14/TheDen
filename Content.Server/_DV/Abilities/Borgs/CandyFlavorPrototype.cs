// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Nutrition;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.Abilities.Borgs;

/// <summary>
/// Describes the color and flavor profile of lollipops and gumballs. Yummy!
/// </summary>
[Prototype("candyFlavor")]
public sealed partial class CandyFlavorPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The display name for this candy. Not localized.
    /// </summary>
    [DataField] public string Name { get; private set; } = "";

    /// <summary>
    /// The color of the candy.
    /// </summary>
    [DataField] public Color Color { get; private set; } = Color.White;

    /// <summary>
    /// How the candy tastes like.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<FlavorPrototype>> Flavors { get; private set; } = [];
}
