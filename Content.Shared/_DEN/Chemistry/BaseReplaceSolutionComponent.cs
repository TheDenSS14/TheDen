// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._DEN.Chemistry;

/// <summary>
/// Entities with this component may partially convert reagents into other reagents under certain conditions.
/// </summary>
public abstract partial class BaseReplaceSolutionComponent : Component
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
    public List<SolutionReplacement> Replacements = default!;

    /// <summary>
    /// Gets a list of all reagent IDs that are considering "rot byproducts", and thus
    /// should not be replaced.
    /// </summary>
    /// <returns>List of rot byproduct reagent IDs.</returns>
    public HashSet<ReagentId> ReplacementReagentIds()
    {
        HashSet<ReagentId> reagentIds = new HashSet<ReagentId>();

        foreach (var replacement in Replacements)
        {
            if (replacement.ReplacementSolution.Contents.Count == 0)
                continue;

            foreach (var reagent in replacement.ReplacementSolution.Contents)
            {
                reagentIds.Add(reagent.Reagent);
            }
        }

        return reagentIds;
    }
}

/// <summary>
/// An entity that will replace its solution every given interval.
/// </summary>
public abstract partial class BaseReplaceSolutionIntervalComponent : BaseReplaceSolutionComponent
{
    /// <summary>
    /// How long it takes to replace the solution once.
    /// </summary>
    [DataField("duration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Duration = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The time when the next replacement will occur.
    /// </summary>
    [DataField("nextChargeTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextReplaceTime = TimeSpan.FromSeconds(0);
}

/// <summary>
/// A data definition for specifying what reagents get replaced, and how much.
/// </summary>

[DataDefinition]
public sealed partial class SolutionReplacement
{
    /// <summary>
    /// The reagent(s) to be replaced in the solution.
    /// </summary>
    [DataField("solution", required: true), ViewVariables(VVAccess.ReadWrite)]
    public Solution ReplacementSolution = default!;

    /// <summary>
    /// How much of the original solution should be reduced each cycle.
    /// </summary>

    [DataField("amount", required: true), ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 Amount = FixedPoint2.Zero;

    /// <summary>
    /// When specified, this only replaces the given reagents.
    /// </summary>
    [DataField("whitelist"), ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<ReagentPrototype>>? Whitelist = default!;
}
