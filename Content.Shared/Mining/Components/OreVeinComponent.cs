// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mining;
using Content.Shared.Random;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Mining.Components;

/// <summary>
/// Defines an entity that will drop a random ore after being destroyed.
/// </summary>
[RegisterComponent]
public sealed partial class OreVeinComponent : Component
{
    /// <summary>
    /// How often an entity will be seeded with ore. Note: the amount of ore
    /// that is dropped is dependent on the ore prototype. <see crefalso="OrePrototype"/>
    /// </summary>
    [DataField("oreChance")]
    public float OreChance = 0.1f;

    /// <summary>
    /// The weighted random prototype used for determining what ore will be dropped.
    /// </summary>
    [DataField("oreRarityPrototypeId", customTypeSerializer: typeof(PrototypeIdSerializer<WeightedRandomOrePrototype>))]
    public string? OreRarityPrototypeId;

    /// <summary>
    /// The ore that this entity holds.
    /// If set in the prototype, it will not be overriden.
    /// </summary>
    [DataField("currentOre", customTypeSerializer: typeof(PrototypeIdSerializer<OrePrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string? CurrentOre;
}
