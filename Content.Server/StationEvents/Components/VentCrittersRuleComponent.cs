// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Nim
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 Jakumba
// SPDX-FileCopyrightText: 2025 empty0set
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Server.StationEvents.Events;
using Content.Shared.EntityTable.EntitySelectors;
using Content.Shared.Storage;
using Robust.Shared.Audio;
using Robust.Shared.Map; // DeltaV

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(VentCrittersRule))]
public sealed partial class VentCrittersRuleComponent : Component
{
    // DeltaV: Replaced by Table
    //[DataField("entries")]
    //public List<EntitySpawnEntry> Entries = new();

    /// <summary>
    /// DeltaV: Table of possible entities to spawn.
    /// </summary>
    [DataField(required: true)]
    public EntityTableSelector Table = default!;

    /// <summary>
    /// At least one special entry is guaranteed to spawn
    /// </summary>
    [DataField("specialEntries")]
    public List<EntitySpawnEntry> SpecialEntries = new();

    /// <summary>
    /// DeltaV: The location of the vent that got picked.
    /// </summary>
    [ViewVariables]
    public EntityCoordinates? Location;

    /// <summary>
    /// DeltaV: Base minimum number of critters to spawn.
    /// </summary>
    [DataField]
    public int Min = 4;

    /// <summary>
    /// DeltaV: Base maximum number of critters to spawn.
    /// </summary>
    [DataField]
    public int Max = 5;

    /// <summary>
    /// DeltaV: Min and max get multiplied by the player count then divided by this.
    /// </summary>
    [DataField]
    public int PlayerRatio = 20;

    [DataField("sound")]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_DEN/VentCritters/vent_harmless_critter.ogg");
}
