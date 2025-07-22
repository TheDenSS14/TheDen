// SPDX-FileCopyrightText: 2025 slarticodefast
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared.Throwing;

/// <summary>
/// Raised on someone when they try to catch an item.
/// </summary>
[ByRefEvent]
public record struct CatchAttemptEvent(EntityUid Item, float CatchChance, bool Cancelled = false);
