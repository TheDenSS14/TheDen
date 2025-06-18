// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Goobstation.Spawn.Components;

/// <summary>
///     Ensures that related entity will be on station (like NTR or BSO lockers) and will be not duplicate.
///     If station have unique entity - item with this component will be deleted.
/// </summary>
[RegisterComponent]
public sealed partial class UniqueEntityCheckerComponent : Component
{
    /// <summary>
    ///     Name of marker in UniqueEntityMarker
    /// </summary>
    [DataField]
    public string? MarkerName;
}
