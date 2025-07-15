using Robust.Shared.GameStates;

namespace Content.Shared.Physics;


/// <summary>
/// Add this to stuff that needs to be ignored by the doors, but still needs its other colliders to stay the same
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class IgnoreDoorCollisionComponent : Component;
