using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Bed.Cryostorage.Components;

/// <summary>
/// Marker component that indicates a user has requested not to have their cryo announced.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CryoingSilentlyComponent : Component { }
