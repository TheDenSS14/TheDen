// SPDX-FileCopyrightText: 2021 FoLoKe <36813380+FoLoKe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Sirionaut <148076704+Sirionaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Animals.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Animals.Components

/// <summary>
///     Lets an entity produce milk. Uses hunger if present.
/// </summary>
{
    [RegisterComponent, Access(typeof(UdderSystem))]
    internal sealed partial class UdderComponent : Component
    {
        /// <summary>
        ///     The reagent to produce.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public ProtoId<ReagentPrototype> ReagentId = "Milk";

        /// <summary>
        ///     The name of <see cref="Solution"/>.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public string SolutionName = "udder";

        /// <summary>
        ///     The solution to add reagent to.
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? Solution = null;

        /// <summary>
        ///     The amount of reagent to be generated on update.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public FixedPoint2 QuantityPerUpdate = 25;

        /// <summary>
        ///     The amount of nutrient consumed on update.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float HungerUsage = 10f;

        /// <summary>
        ///     How long to wait before producing.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan GrowthDelay = TimeSpan.FromMinutes(1);

        /// <summary>
        ///     When to next try to produce.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan NextGrowth = TimeSpan.FromSeconds(0);
    }
}
