// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 Raikyr0 <Kurohana@hotmail.com.au>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Systems;
using Content.Server.Floofstation.Traits;
using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Whitelist;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.Components
{
    [RegisterComponent, Access(typeof(StomachSystem), typeof(FoodSystem))]
    public sealed partial class StomachComponent : Component
    {
        /// <summary>
        ///     The next time that the stomach will try to digest its contents.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan NextUpdate;

        /// <summary>
        ///     The interval at which this stomach digests its contents.
        /// </summary>
        [DataField]
        public TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        ///     The solution inside of this stomach this transfers reagents to the body.
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? Solution;

        /// <summary>
        ///     What solution should this stomach push reagents into, on the body?
        /// </summary>
        [DataField]
        public string BodySolutionName = BloodstreamComponent.DefaultChemicalsSolutionName;

        /// <summary>
        ///     Time between reagents being ingested and them being
        ///     transferred to <see cref="BloodstreamComponent"/>
        /// </summary>
        [DataField]
        public TimeSpan DigestionDelay = TimeSpan.FromSeconds(20);

        /// <summary>
        ///     A whitelist for what special-digestible-required foods this stomach is capable of eating.
        /// </summary>
        [DataField]
        [Access(Other = AccessPermissions.ReadWriteExecute)]
        public EntityWhitelist? SpecialDigestible = null;

        /// <summary>
        ///     Used to track how long each reagent has been in the stomach
        /// </summary>
        [ViewVariables]
        public readonly List<ReagentDelta> ReagentDeltas = new();

        /// <summary>
        ///     Used to track quantity changes when ingesting & digesting reagents
        /// </summary>
        public sealed class ReagentDelta
        {
            public readonly ReagentQuantity ReagentQuantity;
            public TimeSpan Lifetime { get; private set; }

            public ReagentDelta(ReagentQuantity reagentQuantity)
            {
                ReagentQuantity = reagentQuantity;
                Lifetime = TimeSpan.Zero;
            }

            public void Increment(TimeSpan delta) => Lifetime += delta;
        }
    }
}
