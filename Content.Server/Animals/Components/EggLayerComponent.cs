// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Sirionaut <148076704+Sirionaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Storage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Animals.Components;

/// <summary>
///     This component handles animals which lay eggs (or some other item) on a timer, using up hunger to do so.
///     It also grants an action to players who are controlling these entities, allowing them to do it manually.
/// </summary>

[RegisterComponent]
public sealed partial class EggLayerComponent : Component
{
    [DataField]
    public EntProtoId EggLayAction = "ActionAnimalLayEgg";

    /// <summary>
    ///     The amount of nutrient consumed on update.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float HungerUsage = 60f;

    /// <summary>
    ///     Minimum cooldown used for the automatic egg laying.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EggLayCooldownMin = 60f;

    /// <summary>
    ///     Maximum cooldown used for the automatic egg laying.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EggLayCooldownMax = 120f;

    /// <summary>
    ///     Set during component init.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float CurrentEggLayCooldown;

    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public List<EntitySpawnEntry> EggSpawn = default!;

    [DataField]
    public SoundSpecifier EggLaySound = new SoundPathSpecifier("/Audio/Effects/pop.ogg");

    [DataField]
    public float AccumulatedFrametime;

    [DataField] public EntityUid? Action;
}
