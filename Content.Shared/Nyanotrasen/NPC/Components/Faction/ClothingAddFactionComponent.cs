// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.NPC.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;


namespace Content.Shared.Nyanotrasen.NPC.Components.Faction
{
    [RegisterComponent]
    /// <summary>
    /// Allows clothing to add a faction to you when you wear it.
    /// </summary>
    public sealed partial class ClothingAddFactionComponent : Component
    {
        public bool IsActive = false;

        /// <summary>
        /// Faction added
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite),
         DataField("faction", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<NpcFactionPrototype>))]
        public string Faction = "";
    }
}
