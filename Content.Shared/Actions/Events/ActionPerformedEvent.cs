// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Actions.Events;

/// <summary>
///     Raised on the action entity when it is used and <see cref="BaseActionEvent.Handled"/>.
/// </summary>
/// <param name="Performer">The entity that performed this action.</param>
[ByRefEvent]
public readonly record struct ActionPerformedEvent(EntityUid Performer);
