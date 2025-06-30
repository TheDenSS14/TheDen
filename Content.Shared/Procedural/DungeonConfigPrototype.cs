// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Procedural.DungeonGenerators;
using Content.Shared.Procedural.PostGeneration;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural;

[Prototype("dungeonConfig")]
public sealed partial class DungeonConfigPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("generator", required: true)]
    public IDunGen Generator = default!;

    /// <summary>
    /// Ran after the main dungeon is created.
    /// </summary>
    [DataField("postGeneration")]
    public List<IPostDunGen> PostGeneration = new();
}
