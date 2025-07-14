// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition.Components
{
    [Access(typeof(SharedCreamPieSystem))]
    [RegisterComponent]
    public sealed partial class CreamPiedComponent : Component
    {
        [ViewVariables]
        public bool CreamPied { get; set; } = false;

        /// <summary>
        ///     DEN: When CreamPiedComponent is activated, their creampied sprite visuals are replaced with this.
        ///     Make sure not to put in a `map` or `visible` value here, or it is bound to break something.
        ///     Alternatively, you can ignore this if you're already defining the sprite in layer order.
        /// </summary>
        [DataField]
        public PrototypeLayerData? Layer;

        /// <summary>
        ///     This is the sprite map key that should be replaced when this CreamPiedComponent activates.
        /// </summary>
        [DataField]
        public string LayerKey = "clownedon";
    }

    [Serializable, NetSerializable]
    public enum CreamPiedVisuals
    {
        Creamed,
    }
}
