namespace Content.Server.Atmos.Components

{
    [RegisterComponent]
    public sealed partial class ContentProtectionComponent : Component
    {
        /// <summary>
        /// Whether or not it protects the contents from being lit ablaze
        /// </summary>
        [DataField]
        public bool PreventsIgnition = true;
    }
}
