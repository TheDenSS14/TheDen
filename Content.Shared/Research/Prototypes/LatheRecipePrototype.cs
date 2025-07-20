// SPDX-FileCopyrightText: 2019 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2019 ZelteHonor
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Leon Friedrich
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2021 metalgearsloth
// SPDX-FileCopyrightText: 2022 Paul Ritter
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Moony
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 Tayrtahn
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Lathe.Prototypes;
using Content.Shared.Materials;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Research.Prototypes
{
    [Prototype]
    public sealed partial class LatheRecipePrototype : IPrototype, IInheritingPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        /// <inheritdoc/>
        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<LatheRecipePrototype>))]
        public string[]? Parents { get; }

        /// <inheritdoc />
        [NeverPushInheritance]
        [AbstractDataField]
        public bool Abstract { get; }

        /// <summary>
        ///     Name displayed in the lathe GUI.
        /// </summary>
        [DataField]
        public LocId? Name;

        /// <summary>
        ///     Short description displayed in the lathe GUI.
        /// </summary>
        [DataField]
        public LocId? Description;

        /// <summary>
        ///     The prototype name of the resulting entity when the recipe is printed.
        /// </summary>
        [DataField]
        public EntProtoId? Result;

        [DataField]
        public Dictionary<ProtoId<ReagentPrototype>, FixedPoint2>? ResultReagents;

        /// <summary>
        ///     An entity whose sprite is displayed in the ui in place of the actual recipe result.
        /// </summary>
        [DataField]
        public SpriteSpecifier? Icon;

        [DataField("completetime")]
        public TimeSpan CompleteTime = TimeSpan.FromSeconds(5);

        /// <summary>
        ///     The materials required to produce this recipe.
        ///     Takes a material ID as string.
        /// </summary>
        [DataField]
        public Dictionary<ProtoId<MaterialPrototype>, int> Materials = new();

        [DataField]
        public bool ApplyMaterialDiscount = true;

        /// <summary>
        /// A category used for visually sorting lathe recipes in the UI.
        /// </summary>
        [DataField]
        public ProtoId<LatheCategoryPrototype>? Category;

        /// <summary>
        /// DeltaV: Number of mining points this recipe adds to an oreproc when printed.
        /// Scales with stack count.
        /// </summary>
        [DataField]
        public uint MiningPoints;
    }
}