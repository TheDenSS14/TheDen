﻿namespace Content.Server._DEN.Bed.Components
{
    /// <summary>
    ///     Entities in the critical state strapped to this bed will receive stabilizing effects.
    /// </summary>
    [RegisterComponent]
    public sealed partial class StabilizeOnBuckleComponent : Component
    {
        /// <summary>
        ///     How much Asphyxiation and Bloodloss are prevented in the critical state.
        ///     1 = All damage prevented, 0 = no stabilization.
        /// </summary>
        [DataField]
        public float Efficiency = 1f;

        /// <summary>
        ///     How much bleeding it stops.
        /// </summary>
        [DataField]
        public float ReducesBleeding = 0f;

        [DataField] public EntityUid? SleepAction;
    }
}
