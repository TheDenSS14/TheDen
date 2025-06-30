// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.FixedPoint;

namespace Content.Server.Body.Events;

/// <summary>
///     Raised on a mob when its bloodstream tries to perform natural blood regeneration.
/// </summary>
[ByRefEvent]
public sealed class NaturalBloodRegenerationAttemptEvent : CancellableEntityEventArgs
{
    /// <summary>
    ///     How much blood the mob will regenerate on this tick. Can be negative.
    /// </summary>
    public FixedPoint2 Amount;
}
