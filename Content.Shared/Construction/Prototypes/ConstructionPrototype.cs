// SPDX-FileCopyrightText: 2019 Silver
// SPDX-FileCopyrightText: 2020 Swept
// SPDX-FileCopyrightText: 2020 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2020 py01
// SPDX-FileCopyrightText: 2020 zumorica
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2022 Kara D
// SPDX-FileCopyrightText: 2022 Morb
// SPDX-FileCopyrightText: 2022 Paul Ritter
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 08A
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 PixelTK
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Ed
// SPDX-FileCopyrightText: 2025 ArtisticRoomba
// SPDX-FileCopyrightText: 2025 chromiumboy
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Construction.Conditions;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Shared.Construction.Prototypes;

[Prototype("construction")]
public sealed partial class ConstructionPrototype : IPrototype
{
    [DataField("conditions")] private List<IConstructionCondition> _conditions = new();

    /// <summary>
    ///     Hide from the construction list
    /// </summary>
    [DataField("hide")]
    public bool Hide = false;

    /// <summary>
    ///     Friendly name displayed in the construction GUI.
    /// </summary>
    [DataField("name")]
    public string Name = string.Empty;

    /// <summary>
    ///     "Useful" description displayed in the construction GUI.
    /// </summary>
    [DataField("description")]
    public string Description = string.Empty;

    /// <summary>
    ///     The <see cref="ConstructionGraphPrototype"/> this construction will be using.
    /// </summary>
    [DataField("graph", customTypeSerializer: typeof(PrototypeIdSerializer<ConstructionGraphPrototype>), required: true)]
    public string Graph = string.Empty;

    /// <summary>
    ///     The target <see cref="ConstructionGraphNode"/> this construction will guide the user to.
    /// </summary>
    [DataField("targetNode")]
    public string TargetNode = string.Empty;

    /// <summary>
    ///     The starting <see cref="ConstructionGraphNode"/> this construction will start at.
    /// </summary>
    [DataField("startNode")]
    public string StartNode = string.Empty;

    /// <summary>
    ///     Texture path inside the construction GUI.
    /// </summary>
    [DataField("icon")]
    public SpriteSpecifier Icon = SpriteSpecifier.Invalid;

    /// <summary>
    ///     Texture paths used for the construction ghost.
    /// </summary>
    [DataField("layers")]
    private List<SpriteSpecifier>? _layers;

    /// <summary>
    ///     If you can start building or complete steps on impassable terrain.
    /// </summary>
    [DataField("canBuildInImpassable")]
    public bool CanBuildInImpassable { get; private set; }

    /// <summary>
    /// If not null, then this is used to check if the entity trying to construct this is whitelisted.
    /// If they're not whitelisted, hide the item.
    /// </summary>
    [DataField("entityWhitelist")]
    public EntityWhitelist? EntityWhitelist = null;

    [DataField("category")] public string Category { get; private set; } = "";

    [DataField("objectType")] public ConstructionType Type { get; private set; } = ConstructionType.Structure;

    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("placementMode")]
    public string PlacementMode = "PlaceFree";

    /// <summary>
    ///     Whether this construction can be constructed rotated or not.
    /// </summary>
    [DataField("canRotate")]
    public bool CanRotate = true;

    /// <summary>
    ///     Construction to replace this construction with when the current one is 'flipped'
    /// </summary>
    [DataField("mirror", customTypeSerializer: typeof(PrototypeIdSerializer<ConstructionPrototype>))]
    public string? Mirror;

    /// <summary>
    ///     Possible constructions to replace this one with as determined by the placement mode
    /// </summary>
    [DataField]
    public ProtoId<ConstructionPrototype>[] AlternativePrototypes = [];

    public IReadOnlyList<IConstructionCondition> Conditions => _conditions;
    public IReadOnlyList<SpriteSpecifier> Layers => _layers ?? new List<SpriteSpecifier> { Icon };
}

public enum ConstructionType
{
    Structure,
    Item,
}
