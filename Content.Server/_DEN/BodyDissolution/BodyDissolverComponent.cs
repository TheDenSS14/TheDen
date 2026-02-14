namespace Content.Server.BodyDissolution
{
    [RegisterComponent]
    public sealed partial class BodyDissolverComponent : Component
    {
        /// <summary>
        ///     Will this refuse to dissolve a living mob?
        /// </summary>
        [DataField]
        public bool SafetyEnabled = true;
    }
}
