// SPDX-FileCopyrightText: 2022 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 2022 rolfero <45628623+rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Power.Components
{

    [RegisterComponent]
    public sealed partial class UpgradeBatteryComponent : Component
    {
        /// <summary>
        ///     The machine part that affects the power capacity.
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string MachinePartPowerCapacity = "PowerCell";

        /// <summary>
        ///     The machine part rating is raised to this power when calculating power gain
        /// </summary>
        [DataField]
        public float MaxChargeMultiplier = 2f;

        /// <summary>
        ///     Power gain scaling
        /// </summary>
        [DataField]
        public float BaseMaxCharge = 8000000;
    }
}
