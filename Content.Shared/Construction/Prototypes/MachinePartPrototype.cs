// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Construction.Prototypes;

/// <summary>
/// This is a prototype for categorizing
/// different types of machine parts.
/// </summary>
[Prototype("machinePart")]
public sealed partial class MachinePartPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// A human-readable name for the machine part type.
    /// </summary>
    [DataField("name")]
    public string Name = string.Empty;

    /// <summary>
    /// A stock part entity based on the machine part.
    /// </summary>
    [DataField("stockPartPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>), required: true)]
    public string StockPartPrototype = string.Empty;
}
