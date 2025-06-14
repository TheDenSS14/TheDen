using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Chat;


[Serializable, NetSerializable]
public sealed class EmotesVolumeChanged : EntityEventArgs
{
    public float EmotesVolumeMultiplier { get; set; }

    public EmotesVolumeChanged(float emotesVolumeMultiplier)
    {
        EmotesVolumeMultiplier = emotesVolumeMultiplier;
    }
}
