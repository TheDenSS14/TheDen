namespace Content.Shared._DEN.Earmuffs;


/// <summary>
/// This handles the ability to only hear in a certain radius.
/// </summary>
public abstract class SharedEarmuffsSystem : EntitySystem;

public sealed class EarmuffsUpdated(int hearRangePercentage) : EntityEventArgs
{
    public int HearRangePercentage { get; set; } = hearRangePercentage;
}
