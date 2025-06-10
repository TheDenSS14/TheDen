using Robust.Shared.GameStates;

namespace Content.Shared._DEN.Chemistry;

/// <summary>
/// A component that works in tandem with <see cref="RottingComponent"/>. When this entity is rotten,
/// the solution with the given name will be replaced with a given other solution.
/// For example, rotting corpses may build up gastrotoxin.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ReplaceSolutionWhenRottenComponent : BaseReplaceSolutionIntervalComponent
{ }
