// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Research.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Lathe.Prototypes;

/// <summary>
/// This is a prototype for a category for <see cref="LatheRecipePrototype"/>
/// </summary>
[Prototype]
public sealed partial class LatheCategoryPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    /// <summary>
    /// A localized string used in the UI
    /// </summary>
    [DataField]
    public LocId Name;
}
