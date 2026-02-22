using Content.Shared.Atmos;

namespace Content.Server.BodyDissolution
{
    [RegisterComponent]
    public sealed partial class BodyDissolvableComponent : Component
    {
        /// <summary>
        /// What gas should the entity emit upon being dissolved?
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public Gas EmittedGas = Gas.Ammonia;

        /// <summary>
        /// Coefficient for multiplying the mass of the dissolved entity in order to get the amount of moles of gas emitted.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float EmittedGasCoefficient = 0.5f; // this results in about 100 moles for a standard xeno which is a moderate plume

        /// <summary>
        /// The maximum size of the puddle created upon the entity being dissolved.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float MaximumSpillAmount = 20f; // 20 by default as this the maximum amount before reagents spill over into another tile
    }
}
