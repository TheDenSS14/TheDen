using Content.Server.Objectives.Components;

namespace Content.Server._Floof.Traits.Components;

/// <summary>
///     Marks this player as eligible for being the target of
///     chosen types of antagonist objectives.
/// </summary>
[RegisterComponent]
public sealed partial class MarkedComponent : Component
{
    [DataField, ViewVariables]
    public ObjectiveTypes TargetType;
}
