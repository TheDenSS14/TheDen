// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Riggle <27156122+RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.Notes;

[Serializable, NetSerializable]
public sealed class AdminMessageEuiState(TimeSpan time, AdminMessageEuiState.Message[] messages) : EuiStateBase
{
    public TimeSpan Time { get; } = time;
    public Message[] Messages { get; } = messages;

    [Serializable]
    public sealed class Message(string text, string adminName, DateTime addedOn)
    {
        public string Text = text;
        public string AdminName = adminName;
        public DateTime AddedOn = addedOn;
    }
}

public static class AdminMessageEuiMsg
{
    [Serializable, NetSerializable]
    public sealed class Dismiss(bool permanent) : EuiMessageBase
    {
        public bool Permanent { get; } = permanent;
    }
}
