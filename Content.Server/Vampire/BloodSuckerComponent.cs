// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DEN.Body.Systems;

namespace Content.Server.Vampiric
{
    [RegisterComponent]
    public sealed partial class BloodSuckerComponent : Component
    {
        /// <summary>
        /// How much to succ each time we succ.
        /// </summary>
        [DataField("unitsToSucc")]
        public float UnitsToSucc = 20f;

        /// <summary>
        /// The time (in seconds) that it takes to succ an entity.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan Delay = TimeSpan.FromSeconds(4);

        // ***INJECT WHEN SUCC***

        /// <summary>
        /// Whether to inject chems into a chemstream when we suck something.
        /// </summary>
        [DataField("injectWhenSucc")]
        public bool InjectWhenSucc = false;

        /// <summary>
        /// How many units of our injected chem to inject.
        /// </summary>
        [DataField("unitsToInject")]
        public float UnitsToInject = 5;

        /// <summary>
        /// Which reagent to inject.
        /// </summary>
        [DataField("injectReagent")]
        public string InjectReagent = "";

        /// <summary>
        /// Whether we need to web the thing up first...
        /// </summary>
        [DataField("webRequired")]
        public bool WebRequired = false;

        /// <summary>
        ///     DEN: Used to track BloodExaminer, but only if it is added by this component.
        /// </summary>
        public BloodExaminerComponent? AddedBloodExaminer;
    }
}
