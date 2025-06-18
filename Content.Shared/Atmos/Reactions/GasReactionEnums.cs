// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Atmos.Reactions;

[Flags]
public enum ReactionResult : byte
{
    NoReaction = 0,
    Reacting = 1,
    StopReactions = 2,
}

public enum GasReaction : byte
{
    Fire = 0,
}
