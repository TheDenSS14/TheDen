// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     Networked event from client.
///     Send to server when client started/stopped typing in chat input field.
/// </summary>
[Serializable, NetSerializable]
public sealed class TypingChangedEvent : EntityEventArgs
{
    public readonly bool IsTyping;

    public TypingChangedEvent(bool isTyping)
    {
        IsTyping = isTyping;
    }
}
