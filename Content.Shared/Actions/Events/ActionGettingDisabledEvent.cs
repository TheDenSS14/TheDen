// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Actions.Events;

/// <summary>
///     Raised on the action entity when it is getting disabled on <see cref="Performer">performer</see>.
/// </summary>
/// <param name="Performer">The entity that performed this action.</param>
[ByRefEvent]
public readonly record struct ActionGettingDisabledEvent(EntityUid Performer);
