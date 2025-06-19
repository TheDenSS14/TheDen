// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Ygg01 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using Robust.Shared.Utility;

namespace Content.Shared.Procedural;

[Prototype("dungeonRoom")]
public sealed partial class DungeonRoomPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = string.Empty;

    [ViewVariables(VVAccess.ReadWrite), DataField("tags", customTypeSerializer:typeof(PrototypeIdListSerializer<TagPrototype>))]
    public List<string> Tags = new();

    [DataField("size", required: true)] public Vector2i Size;

    /// <summary>
    /// Path to the file to use for the room.
    /// </summary>
    [DataField("atlas", required: true)] public ResPath AtlasPath;

    /// <summary>
    /// Tile offset into the atlas to use for the room.
    /// </summary>
    [DataField("offset", required: true)] public Vector2i Offset;
}
