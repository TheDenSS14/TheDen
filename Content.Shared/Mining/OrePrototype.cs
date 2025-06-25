// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Alekshhh <44923899+Alekshhh@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Mining;

/// <summary>
/// This is a prototype for defining ores that generate in rock
/// </summary>
[Prototype("ore")]
public sealed partial class OrePrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("oreEntity", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string? OreEntity;

    [DataField("minOreYield")]
    public int MinOreYield = 1;

    [DataField("maxOreYield")]
    public int MaxOreYield = 1;

    //TODO: add sprites for ores for things like mining analyzer
}
