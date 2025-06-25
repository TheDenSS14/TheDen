// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Antag.Components;

/// <summary>
/// Spawns this rule's antags at random tiles on a station using <c>TryGetRandomTile</c>.
/// Requires <see cref="AntagSelectionComponent"/>.
/// </summary>
[RegisterComponent]
public sealed partial class AntagRandomSpawnComponent : Component;
