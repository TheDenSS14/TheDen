// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 brainfood1183
// SPDX-FileCopyrightText: 2025 Vanessa Louwagie
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.Components
{
    [RegisterComponent]
    [NetworkedComponent]
    public sealed partial class UnremoveableComponent : Component
    {
        /// <summary>
        /// If this is true then unremovable items that are removed from inventory are deleted (typically from corpse gibbing).
        /// Items within unremovable containers are not deleted when removed.
        /// </summary>
        [DataField]
        public bool DeleteOnDrop = true;
    }
}
