namespace Content.Shared._DEN.Movement.Components;

/// <summary>
///     A component given to items that allow characters with missing legs to stand up instead of being forced to crawl,
/// </summary>

[RegisterComponent]
public sealed partial class SupportStandingComponent : Component
{
    /// <summary>
    ///     The minimum number of legs needed in order to provide standing support.
    /// </summary>
    [DataField]
    public int MinimumLegCount = 1;

    /// <summary>
    ///     Whether this item provides standing support when held in the hand.
    /// </summary>
    [DataField]
    public bool WorksInHand = false;

    /// <summary>
    ///     Whether this item provides standing support when worn.
    /// </summary>
    [DataField]
    public bool WorksWhenEquipped = true;
}
