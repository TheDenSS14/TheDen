// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Announcements.Events;


[Serializable, NetSerializable]
public sealed class AnnouncementSendEvent : EntityEventArgs
{
    public string AnnouncerId { get; }
    public string AnnouncementId { get; }
    public List<NetUserId> Recipients { get; }
    public AudioParams AudioParams { get; }

    public AnnouncementSendEvent(string announcerId, string announcementId, List<NetUserId> recipients, AudioParams audioParams)
    {
        AnnouncerId = announcerId;
        AnnouncementId = announcementId;
        Recipients = recipients;
        AudioParams = audioParams;
    }
}
