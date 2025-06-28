using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Species;


/// <summary>
/// This is used for making a user exempt to species height/width restrictions.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SpeciesRestrictionExemptComponent : Component;
