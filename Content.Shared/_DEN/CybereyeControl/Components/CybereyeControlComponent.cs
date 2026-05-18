using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Cybereye.Components;

/// <summary>
/// A bandaid fix for not being able to turn off the HUD which cybereyes provide. 
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CybereyeControlComponent : BaseCybereyeControlComponent { }