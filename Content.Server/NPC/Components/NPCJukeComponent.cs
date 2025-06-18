// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.NPC.Components;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class NPCJukeComponent : Component
{
    [DataField]
    public JukeType JukeType = JukeType.Away;

    [DataField]
    public float JukeDuration = 0.5f;

    [DataField]
    public float JukeCooldown = 3f;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextJuke;

    [DataField]
    public Vector2i? TargetTile;
}

public enum JukeType : byte
{
    /// <summary>
    /// Will move directly away from target if applicable.
    /// </summary>
    Away,

    /// <summary>
    /// Move to the adjacent tile for the specified duration.
    /// </summary>
    AdjacentTile
}
