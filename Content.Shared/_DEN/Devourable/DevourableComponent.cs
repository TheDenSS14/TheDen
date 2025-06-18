using Robust.Shared.GameStates;


namespace Content.Shared._DEN.Devourable;


/// <summary>
/// This is used for ensuring dragon devour consent.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DevourableComponent : Component
{

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(5);
    public TimeSpan LastUpdateTime { get; set; } = TimeSpan.Zero;

    [DataField]
    public bool IsDevourable { get; set; }

    [DataField]
    public bool AttemptedDevouring { get; set; }
}
