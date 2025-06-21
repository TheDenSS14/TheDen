// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Containers.ItemSlots;
using Content.Shared.RadialSelector;

namespace Content.Server.WhiteDream.BloodCult.Constructs.Shell;

[RegisterComponent]
public sealed partial class ConstructShellComponent : Component
{
    [DataField(required: true)]
    public ItemSlot ShardSlot = new();

    public readonly string ShardSlotId = "Shard";

    [DataField]
    public List<RadialSelectorEntry> Constructs = new()
    {
        new() { Prototype = "ConstructJuggernaut", },
        new() { Prototype = "ConstructArtificer", },
        new() { Prototype = "ConstructWraith", }
    };

    [DataField]
    public List<RadialSelectorEntry> PurifiedConstructs = new()
    {
        new() { Prototype = "ConstructJuggernautHoly", },
        new() { Prototype = "ConstructArtificerHoly", },
        new() { Prototype = "ConstructWraithHoly", }
    };
}
