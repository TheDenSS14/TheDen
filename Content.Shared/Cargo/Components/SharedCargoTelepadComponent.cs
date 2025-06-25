// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Construction.Prototypes;
using Content.Shared.DeviceLinking;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Cargo.Components;

/// <summary>
/// Handles teleporting in requested cargo after the specified delay.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedCargoSystem))]
public sealed partial class CargoTelepadComponent : Component
{
    [DataField]
    public List<CargoOrderData> CurrentOrders = new();

    /// <summary>
    ///     The base amount of time it takes to teleport from the telepad
    /// </summary>
    [DataField]
    public float BaseDelay = 10f;

    /// <summary>
    ///     The actual amount of time it takes to teleport from the telepad
    /// </summary>
    [DataField]
    public float Delay = 10f;

    /// <summary>
    ///     The machine part that affects <see cref="Delay"/>
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string MachinePartTeleportDelay = "Capacitor";

    /// <summary>
    ///     A multiplier applied to <see cref="Delay"/> for each level of <see cref="MachinePartTeleportDelay"/>
    /// </summary>
    [DataField]
    public float PartRatingTeleportDelay = 0.8f;

    /// <summary>
    ///     How much time we've accumulated until next teleport.
    /// </summary>
    [DataField]
    public float Accumulator;

    [DataField]
    public CargoTelepadState CurrentState = CargoTelepadState.Unpowered;

    [DataField]
    public SoundSpecifier TeleportSound = new SoundPathSpecifier("/Audio/Machines/phasein.ogg");

    /// <summary>
    ///     The paper-type prototype to spawn with the order information.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string PrinterOutput = "PaperCargoInvoice";

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string ReceiverPort = "OrderReceiver";
}
