using Robust.Shared.GameStates;


namespace Content.Shared.RpConsume;

/// <summary>
/// Adds the ability to use and 'consume' an item without doing anything.
/// Displays the associated text when the doAfter starts.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(RpConsumeSystem))]
public sealed partial class RpConsumeComponent : Component
{
    /// <summary>
    /// The text to be popped up when the item is used.
    /// </summary>
    [DataField(required: true)]
    public LocId ConsumePopup;

    /// <summary>
    /// How long it takes to 'consume' the item.
    /// </summary>
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(1.5f);
}
