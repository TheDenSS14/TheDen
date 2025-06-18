// SPDX-FileCopyrightText: 2024 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Pulling.Events;

/// <summary>
/// Raised when a request is made to stop pulling an entity.
/// </summary>
public record struct AttemptStopPullingEvent(EntityUid? User = null)
{
    public readonly EntityUid? User = User;
    public bool Cancelled;
}