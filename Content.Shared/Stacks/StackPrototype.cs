// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Shared.Stacks;

[Prototype("stack")]
public sealed partial class StackPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Human-readable name for this stack type e.g. "Steel"
    /// </summary>
    /// <remarks>This is a localization string ID.</remarks>
    [DataField("name")]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    ///     An icon that will be used to represent this stack type.
    /// </summary>
    [DataField("icon")]
    public SpriteSpecifier? Icon { get; private set; }

    /// <summary>
    ///     The entity id that will be spawned by default from this stack.
    /// </summary>
    [DataField("spawn", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string Spawn { get; private set; } = string.Empty;

    /// <summary>
    ///     The maximum amount of things that can be in a stack.
    ///     Can be overriden on <see cref="StackComponent"/>
    ///     if null, simply has unlimited max count.
    /// </summary>
    [DataField("maxCount")]
    public int? MaxCount { get; private set; }

    /// <summary>
    /// The size of an individual unit of this stack.
    /// </summary>
    [DataField("itemSize")]
    public int? ItemSize;
}

