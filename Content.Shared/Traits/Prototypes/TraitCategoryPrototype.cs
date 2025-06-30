// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Traits;


/// <summary>
///     A prototype defining a valid category for <see cref="TraitPrototype"/>s to go into.
/// </summary>
[Prototype("traitCategory")]
public sealed partial class TraitCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public bool Root;

    [DataField]
    public List<ProtoId<TraitCategoryPrototype>> SubCategories = new();
}
