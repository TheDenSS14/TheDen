// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.Salvage;
using Content.Shared.Salvage.Expeditions;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Server.Salvage.Expeditions;

/// <summary>
/// Designates this entity as holding a salvage expedition.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class SalvageExpeditionComponent : SharedSalvageExpeditionComponent
{
    public SalvageMissionParams MissionParams = default!;

    /// <summary>
    /// Where the dungeon is located for initial announcement.
    /// </summary>
    [DataField("dungeonLocation")]
    public Vector2 DungeonLocation = Vector2.Zero;

    /// <summary>
    /// When the expeditions ends.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan EndTime;

    /// <summary>
    /// Station whose mission this is.
    /// </summary>
    [DataField("station")]
    public EntityUid Station;

    [ViewVariables] public bool Completed = false;

    /// <summary>
    /// Countdown audio stream.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Stream = null;

    /// <summary>
    /// Sound that plays when the mission end is imminent.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("ExpeditionEnd")
    {
        Params = AudioParams.Default.WithVolume(-5),
    };

    /// <summary>
    /// Song selected on MapInit so we can predict the audio countdown properly.
    /// </summary>
    [DataField]
    public SoundPathSpecifier SelectedSong;
}
