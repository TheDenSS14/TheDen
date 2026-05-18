using Content.Shared.Overlays;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.Cybereye.Components;

/// <summary>
/// A bandaid fix for not being able to turn off the HUD which cybereyes provide. 
/// </summary>
[RegisterComponent, NetworkedComponent]
public abstract partial class BaseCybereyeControlComponent : Component
{
    [DataField]
    public EntProtoId Action = "CybereyeControlAction";
    public EntityUid? ActionEntity;

    [DataField]
    public ComponentRegistry RemoveOnToggle = new ComponentRegistry();

    /// <summary>
    /// Is the entity's cybereye currently on?
    /// </summary>
    [DataField]
    public bool Active = true;
}