namespace Content.Server.Language;

// <summary>
//     The language used for Hypnoglossy, a roleplay variant of Psychomantic with its own prototype
// </summary>
[RegisterComponent]
public sealed partial class PsychomandateSpeakerComponent : Component
{
    [DataField]
    public bool Enabled = true;
}
