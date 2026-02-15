using Robust.Shared.Audio;

namespace Content.Server.BodyDissolution
{
    [RegisterComponent]
    public sealed partial class BodyDissolverComponent : Component
    {
        /// <summary>
        /// Sound to play after embedding into a hit target.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
        public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_DEN/Effects/body_dissolver_tack.ogg");

        /// <summary>
        ///     Will this refuse to dissolve a living mob?
        /// </summary>
        [DataField]
        public bool SafetyEnabled = true;
    }
}
