// SPDX-FileCopyrightText: 2024 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Physics.Components;

namespace Content.Shared.Movement.Pulling.Events;

/// <summary>
/// Raised directed on puller and pullable to determine if it can be pulled.
/// </summary>
public sealed class PullAttemptEvent : PullMessage
{
    public PullAttemptEvent(EntityUid pullerUid, EntityUid pullableUid) : base(pullerUid, pullableUid) { }

    public bool Cancelled { get; set; }
}
