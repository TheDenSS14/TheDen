// SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Construction.Components;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Power.Components;

[RegisterComponent]
public sealed partial class UpgradePowerSupplyRampingComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public float BaseRampRate;

    /// <summary>
    ///     The machine part that affects the power supply ramping
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
    public string MachinePartRampRate = "Capacitor";

    /// <summary>
    ///     The multiplier used for scaling the power supply ramping
    /// </summary>
    [DataField]
    public float SupplyRampingMultiplier = 1f;

    /// <summary>
    ///     What type of scaling is being used?
    /// </summary>
    [DataField(required: true)]
    public MachineUpgradeScalingType Scaling;

    /// <summary>
    ///     The current value that the power supply is being scaled by
    /// </summary>
    [DataField]
    public float ActualScalar = 1f;
}
