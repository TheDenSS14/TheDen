using Robust.Shared.Audio;


namespace Content.Shared._DEN.TimedDespawn;


/// <summary>
/// This is used for a more detailed timed despawn component.
/// </summary>
[RegisterComponent]
public sealed partial class TimedDespawnDetailedComponent : Component
{
    [DataField]
    public TimeSpan StartTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// The amount of time it lasts, in seconds.
    /// </summary>
    [DataField]
    public int DespawnAfter { get; set; }
    [DataField("examineText")]
    public LocId? ExamineLocId { get; set; }

    [DataField]
    public SoundSpecifier? StartSound { get; set; }

    [DataField]
    public AudioParams StartSoundParams { get; set; } = AudioParams.Default;

    [DataField]
    public SoundSpecifier? EndSound { get; set; }

    [DataField]
    public AudioParams EndSoundParams { get; set; } = AudioParams.Default;

}
