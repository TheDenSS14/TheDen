// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DEN.Chemistry;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._DEN.Chemistry.EntitySystems;

public sealed class ReplaceSolutionSystem : SharedReplaceSolutionSystem
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        UpdateRottingEntities();
    }

    private void UpdateRottingEntities()
    {
        var query = EntityQueryEnumerator<ReplaceSolutionWhenRottenComponent,
            RottingComponent,
            SolutionContainerManagerComponent>();

        while (query.MoveNext(out var uid, out var replaceSolution, out var _, out var manager))
        {
            UpdateEntityOnInterval(uid, replaceSolution, manager);
        }
    }

    private void UpdateEntityOnInterval(EntityUid uid,
        BaseReplaceSolutionIntervalComponent comp,
        SolutionContainerManagerComponent manager)
    {
        if (_timing.CurTime < comp.NextReplaceTime)
            return;

        comp.NextReplaceTime = _timing.CurTime + comp.Duration;
        UpdateEntity(uid, comp, manager);
    }

    /// <summary>
    /// Performs replacements on an entity with the components needed to do so.
    /// </summary>
    /// <param name="uid">The ID of the entity to perform replacements on.</param>
    /// <param name="comp">The BaseReplaceSolution component to use.</param>
    /// <param name="manager">The SolutionContainerManager component on the entity.</param>
    private void UpdateEntity(EntityUid uid,
        BaseReplaceSolutionComponent comp,
        SolutionContainerManagerComponent manager)
    {
        var success = _solutionContainer.ResolveSolution((uid, manager),
            comp.SolutionName,
            ref comp.SolutionRef,
            out var solution);

        if (!success || solution == null || comp.SolutionRef == null)
            return;

        var replacedSolution = ReplaceReagents(solution, comp);
        _solutionContainer.RemoveAllSolution(comp.SolutionRef.Value);
        _solutionContainer.AddSolution(comp.SolutionRef.Value, replacedSolution);
    }

    /// <summary>
    /// Performs a replacement of a solution's reagents using the properties of a BaseReplaceSolutionComponent.
    /// For example, the compoment may have a replacement rule that says to reduce the solution volume by 1u,
    /// and then add 1u of Water to the solution.
    /// </summary>
    /// <param name="solution">The solution to perform replacements on</param>
    /// <param name="replaceSolution">The solution replacement component.</param>
    /// <returns>A new solution after replacements have been performed.</returns>
    public Solution ReplaceReagents(Solution solution, BaseReplaceSolutionComponent replaceSolution)
    {
        var replacementTargetIds = replaceSolution.ReplacementReagentIds();
        var replacedProducts = solution.Clone();
        var solutionToReplace = replacedProducts.SplitSolutionWithout(replacedProducts.Volume, replacementTargetIds);

        foreach (var replacement in replaceSolution.Replacements)
        {
            var replacementSolution = PerformReplacement(solutionToReplace, replacement);
            replacedProducts.AddSolution(replacementSolution, _protoMan);
        }

        var finalResultSolution = solutionToReplace;
        finalResultSolution.AddSolution(replacedProducts, _protoMan);
        return finalResultSolution;
    }

    /// <summary>
    /// Given a solution, reduce its volume and then create a solution of "replacement" reagents, scaling with
    /// how much the solution was actually reduced by. Does not perform filtering on the input solution
    /// for "byproducts", so if you fail to filter those out first, behavior may be unpredictable.
    ///
    /// This also modifies the input solution in place.
    /// </summary>
    /// <param name="solution">The solution to perform the placement on.</param>
    /// <param name="replacement">The SolutionReplacement rule defining how to replace reagents.</param>
    /// <param name="replacedOutput">The solution of "replacement" reagents generated.</param>
    public Solution PerformReplacement(Solution solution, SolutionReplacement replacement)
    {
        if (solution.Volume <= 0 || replacement.ReplacementSolution.Volume <= 0)
            return new Solution();

        Solution? ignoredSolution = null;
        if (replacement.Whitelist != null)
            ignoredSolution = solution.SplitSolutionWithout(solution.Volume, replacement.Whitelist);

        var originalVolume = solution.Volume;
        solution.RemoveSolution(replacement.Amount);

        // Important to make sure this scales with how much is actually replaced.
        // For example, if it replaces 2u of Nutriment with 3u of Gastrotoxin,
        // but there was only 1u left of Nutriment in the original solution,
        // then it should only generate 1.5u of Gastrotoxin.
        var replacedAmount = originalVolume - solution.Volume;
        var replacementRatio = replacedAmount.Float() / replacement.Amount.Float();
        var replacementSolution = replacement.ReplacementSolution.Clone();
        replacementSolution.ScaleSolution(replacementRatio);

        // If there's a whitelist, make sure we readd our non-whitelisted solution back.
        if (ignoredSolution != null && ignoredSolution.Volume > 0)
            solution.AddSolution(ignoredSolution, _protoMan);

        return replacementSolution;
    }
}
