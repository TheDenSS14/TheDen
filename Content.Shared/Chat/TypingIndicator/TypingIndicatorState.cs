// SPDX-FileCopyrightText: 2025 88tv
// SPDX-FileCopyrightText: 2025 8tv
// SPDX-FileCopyrightText: 2025 lzk
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Chat.TypingIndicator;

[Serializable, NetSerializable]
public enum TypingIndicatorState
{
    None = 0,
    Idle = 1,
    Typing = 2,
}
