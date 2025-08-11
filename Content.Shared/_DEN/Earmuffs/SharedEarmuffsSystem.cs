using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Earmuffs;


/// <summary>
/// This handles the ability to only hear in a certain radius.
/// </summary>
public abstract class SharedEarmuffsSystem : EntitySystem;

[Serializable, NetSerializable]
public sealed class EarmuffsUpdated(float hearRange) : EntityEventArgs
{
    public float HearRange { get; init; } = hearRange;
}
