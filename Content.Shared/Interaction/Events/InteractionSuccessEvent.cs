// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Interaction.Events;

/// <summary>
/// Raised on the target when successfully petting/hugging something.
/// </summary>
[ByRefEvent]
public readonly record struct InteractionSuccessEvent(EntityUid User);
