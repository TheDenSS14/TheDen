// SPDX-FileCopyrightText: 2023 JJ <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Nyanotrasen.Kitchen.Components
{
    [NetworkedComponent]
    public abstract partial class SharedDeepFriedComponent : Component
    {
        /// <summary>
        /// How deep-fried is this item?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("crispiness")]
        public int Crispiness { get; set; }
    }

    [Serializable, NetSerializable]
    public enum DeepFriedVisuals : byte
    {
        Fried,
    }
}
