// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Audio;

namespace Content.Shared.Psionics.Glimmer
{
    [RegisterComponent]
    public sealed partial class SharedGlimmerReactiveComponent : Component
    {
        /// <summary>
        /// Do the effects of this component require power from an APC?
        /// </summary>
        [DataField("requiresApcPower")]
        public bool RequiresApcPower = false;

        /// <summary>
        /// Does this component try to modulate the strength of a PointLight
        /// component on the same entity based on the Glimmer tier?
        /// </summary>
        [DataField("modulatesPointLight")]
        public bool ModulatesPointLight = false;

        /// <summary>
        /// What is the correlation between the Glimmer tier and how strongly
        /// the light grows? The result is added to the base Energy.
        /// </summary>
        [DataField("glimmerToLightEnergyFactor")]
        public float GlimmerToLightEnergyFactor = 1.0f;

        /// <summary>
        /// What is the correlation between the Glimmer tier and how much
        /// distance the light covers? The result is added to the base Radius.
        /// </summary>
        [DataField("glimmerToLightRadiusFactor")]
        public float GlimmerToLightRadiusFactor = 1.0f;

        /// <summary>
        /// Noises to play on failed turn off.
        /// </summary>
        [DataField("shockNoises")]
        public SoundSpecifier ShockNoises { get; private set; } = new SoundCollectionSpecifier("sparks");
    }
}
