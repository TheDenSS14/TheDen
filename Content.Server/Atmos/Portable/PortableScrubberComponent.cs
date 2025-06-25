// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Portable
{
    [RegisterComponent]
    public sealed partial class PortableScrubberComponent : Component
    {
        /// <summary>
        /// The air inside this machine.
        /// </summary>
        [DataField("gasMixture"), ViewVariables(VVAccess.ReadWrite)]
        public GasMixture Air { get; private set; } = new();

        [DataField("port"), ViewVariables(VVAccess.ReadWrite)]
        public string PortName { get; set; } = "port";

        /// <summary>
        /// Which gases this machine will scrub out.
        /// Unlike fixed scrubbers controlled by an air alarm,
        /// this can't be changed in game.
        /// </summary>
        [DataField("filterGases")]
        public HashSet<Gas> FilterGases = new()
        {
            Gas.CarbonDioxide,
            Gas.Plasma,
            Gas.Tritium,
            Gas.WaterVapor,
            Gas.Ammonia,
            Gas.NitrousOxide,
            Gas.Frezon,
            Gas.BZ, // Assmos - /tg/ gases
            Gas.Healium, // Assmos - /tg/ gases
            Gas.Nitrium, // Assmos - /tg/ gases
            Gas.Hydrogen, // Assmos - /tg/ gases
            Gas.HyperNoblium, // Assmos - /tg/ gases
            Gas.ProtoNitrate, // Assmos - /tg/ gases
            Gas.Zauker, // Assmos - /tg/ gases
            Gas.Halon, // Assmos - /tg/ gases
            Gas.Helium, // Assmos - /tg/ gases
            Gas.AntiNoblium, // Assmos - /tg/ gases
        };

        [ViewVariables(VVAccess.ReadWrite)]
        public bool Enabled = true;

        /// <summary>
        /// Maximum internal pressure before it refuses to take more.
        /// </summary>
        [DataField]
        public float MaxPressure = 2500;

        /// <summary>
        ///     The base amount of maximum internal pressure
        /// </summary>
        [DataField]
        public float BaseMaxPressure = 2500;

        /// <summary>
        ///     The machine part that modifies the maximum internal pressure
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string MachinePartMaxPressure = "MatterBin";

        /// <summary>
        ///     How much the <see cref="MachinePartMaxPressure"/> will affect the pressure.
        ///     The value will be multiplied by this amount for each increasing part tier.
        /// </summary>
        [DataField]
        public float PartRatingMaxPressureModifier = 1.5f;

        /// <summary>
        ///     The speed at which gas is scrubbed from the environment.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float TransferRate = 800;

        /// <summary>
        ///     The base speed at which gas is scrubbed from the environment.
        /// </summary>
        [DataField]
        public float BaseTransferRate = 800;

        /// <summary>
        ///     The machine part which modifies the speed of <see cref="TransferRate"/>
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string MachinePartTransferRate = "Manipulator";

        /// <summary>
        /// How much the <see cref="MachinePartTransferRate"/> will modify the rate.
        /// The value will be multiplied by this amount for each increasing part tier.
        /// </summary>
        [DataField]
        public float PartRatingTransferRateModifier = 1.4f;
    }
}
