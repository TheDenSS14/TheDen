using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.BodyDissolution
{
    [RegisterComponent]
    public sealed partial class BodyDissolverComponent : Component
    {
        /// <summary>
        ///     Sound to play when dissolving a body.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
        public SoundSpecifier? DissolveSound = new SoundPathSpecifier("/Audio/_DEN/Effects/body_dissolver_tack.ogg");

        /// <summary>
        ///     Destroy on use?
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool DestroyOnUse = true;

        /// <summary>
        ///     Can this be emagged?
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool CanBeEmagged = true;

        /// <summary>
        ///     Will this refuse to dissolve a living mob?
        ///     When disabled, will instead deal damage based on the DamageWhenEmagged field, over time.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool SafetyEnabled = true;

        /// <summary>
        ///     Sound to play when emagging the entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
        public SoundSpecifier? EmagSound = new SoundCollectionSpecifier("sparks");

        /// <summary>
        ///     The amount of damage the projectile will do per second while embedded and emagged.
        /// </summary>
        [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier DamageWhenEmagged = new();

        /// <summary>
        ///     How much time before despawning the entity after it embeds itself while emagged.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float EmaggedLifetime = 1f;

        [DataField]
        public TimeSpan DestroyBy = TimeSpan.Zero;
    }
}
