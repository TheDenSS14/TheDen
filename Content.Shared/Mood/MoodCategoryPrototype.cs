// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Mood;

/// <summary>
///     A prototype defining a category for moodlets, where only a single moodlet of a given category is permitted.
/// </summary>
[Prototype]
public sealed partial class MoodCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;
}
