// SPDX-FileCopyrightText: 2022 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Construction.Components;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Power.Components;

/// <summary>
///     This is used for machines whose power draw
///     can be decreased through machine part upgrades.
/// </summary>
[RegisterComponent]
public sealed partial class UpgradePowerDrawComponent : Component
{
    /// <summary>
    ///     The base power draw of the machine.
    ///     Prioritizes hv/mv draw over lv draw.
    ///     Value is initializezd on map init from <see cref="ApcPowerReceiverComponent"/>
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float BaseLoad;

    /// <summary>
    ///     The machine part that affects the power draw.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string MachinePartPowerDraw = "Capacitor";

    /// <summary>
    ///     The multiplier used for scaling the power draw.
    /// </summary>
    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public float PowerDrawMultiplier = 1f;

    /// <summary>
    ///     What type of scaling is being used?
    /// </summary>
    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public MachineUpgradeScalingType Scaling;
}


