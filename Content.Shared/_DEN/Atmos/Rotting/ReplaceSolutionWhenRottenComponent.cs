using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._DEN.Atmos.Rotting;

/// <summary>
/// A component that works in tandem with <see cref="RottingComponent"/>. When this entity is rotten,
/// the solution with the given name will be replaced with a given other solution.
/// For example, rotting corpses may build up gastrotoxin.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
// [AutoGenerateComponentState]
public sealed partial class ReplaceSolutionWhenRottenComponent : Component
{
    /// <summary>
    /// The name of the solution to replace.
    /// </summary>
    [DataField("solution", required: true), ViewVariables(VVAccess.ReadWrite)]
    public string SolutionName = string.Empty;

    /// <summary>
    /// The solution to replace reagents from.
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? SolutionRef = null;

    /// <summary>
    /// A list of reagent replacements to perform. Can have multiple replacements, for example,
    /// replacing every reagent in a solution with something else.
    /// </summary>

    [DataField("replacements"), ViewVariables(VVAccess.ReadWrite)]
    public List<SolutionReplacements> Replacements = default!;

    /// <summary>
    /// How long it takes to replace the solution once.
    /// </summary>
    [DataField("duration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Duration = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The time when the next replacement will occur.
    /// </summary>
    [DataField("nextChargeTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan NextReplaceTime = TimeSpan.FromSeconds(0);
}

[DataDefinition]
public sealed partial class SolutionReplacements
{
    /// <summary>
    /// The reagent(s) to be replaced in the solution.
    /// </summary>
    [DataField("solution", required: true), ViewVariables(VVAccess.ReadWrite)]
    public Solution ReplacementSolution = default!;

    /// <summary>
    /// When specified, this only replaces the given reagents.
    /// </summary>
    [DataField("whitelist"), ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<ReagentPrototype>>? Whitelist = default!;
}
