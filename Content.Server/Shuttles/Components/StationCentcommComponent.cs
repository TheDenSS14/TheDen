// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ArtisticRoomba <145879011+ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Spanky <scott@wearejacob.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Map;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.Components;

/// <summary>
/// Spawns Central Command (emergency destination) for a station.
/// </summary>
[RegisterComponent]
public sealed partial class StationCentcommComponent : Component
{
    /// <summary>
    /// Crude shuttle offset spawning.
    /// </summary>
    [DataField]
    public float ShuttleIndex;

    [DataField]
    public List<ResPath> Maps = new()
    {
        new("/Maps/CentralCommand/main.yml"),
        new("/Maps/CentralCommand/harmony.yml")
    };

    /// <summary>
    /// Centcomm entity that was loaded.
    /// </summary>
    [DataField]
    public EntityUid? Entity;

    [DataField]
    public EntityUid? MapEntity;
}
