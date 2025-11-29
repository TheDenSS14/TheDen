// SPDX-FileCopyrightText: 2019 moneyl
// SPDX-FileCopyrightText: 2020 ike709
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 metalgearsloth
// SPDX-FileCopyrightText: 2022 Paul Ritter
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2024 Kevin Zheng
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Chemistry.Dispenser
{
    /// <summary>
    /// Is simply a list of reagents defined in yaml. This can then be set as a
    /// <see cref="SharedReagentDispenserComponent"/>s <c>pack</c> value (also in yaml),
    /// to define which reagents it's able to dispense. Based off of how vending
    /// machines define their inventory.
    /// </summary>
    [Serializable, NetSerializable, Prototype("reagentDispenserInventory")]
    public sealed partial class ReagentDispenserInventoryPrototype : IPrototype, IInheritingPrototype // DEN: Make inheriting
    {
        // DEN start: Make inheriting
        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<ReagentDispenserInventoryPrototype>))]
        public string[]? Parents { get; }

        [NeverPushInheritance]
        [AbstractDataField]
        public bool Abstract { get; }
        // End DEN

        [DataField("inventory", customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>))]
        [AlwaysPushInheritance] // DEN - Allow partial inheritance of inventory
        public List<string> Inventory = new();

        [ViewVariables, IdDataField]
        public string ID { get; } = default!;
    }
}
