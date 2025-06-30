// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._DV.Paper;

/// <summary>
/// 	Raised on the pen when trying to sign a paper.
/// 	If it's cancelled the signature wasn't made.
/// </summary>
[ByRefEvent]
public record struct SignAttemptEvent(EntityUid Paper, EntityUid User, bool Cancelled = false);
