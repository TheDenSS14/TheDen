using Content.Shared._DEN.Atmos.Rotting;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._DEN.Atmos.EntitySystems;

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

    public void UpdateRottingEntities()
    {
        var query = EntityQueryEnumerator<ReplaceSolutionWhenRottenComponent,
            RottingComponent,
            SolutionContainerManagerComponent>();

        while (query.MoveNext(out var uid, out var replaceSolution, out var _, out var manager))
        {
            if (_timing.CurTime < replaceSolution.NextReplaceTime)
                continue;

            replaceSolution.NextReplaceTime = _timing.CurTime + replaceSolution.Duration;

            var success = _solutionContainer.ResolveSolution((uid, manager),
                replaceSolution.SolutionName,
                ref replaceSolution.SolutionRef,
                out var solution);

            if (!success || solution == null || replaceSolution.SolutionRef == null)
                continue;

            Solution replacedSolution = ReplaceReagents(solution, replaceSolution);
            _solutionContainer.RemoveAllSolution(replaceSolution.SolutionRef.Value);
            _solutionContainer.AddSolution(replaceSolution.SolutionRef.Value, replacedSolution);
        }
    }

    public Solution ReplaceReagents(Solution solution, ReplaceSolutionWhenRottenComponent replaceSolution)
    {
        var replacementTargetIds = replaceSolution.ReplacementReagentIds();
        var replacedProducts = solution.Clone();
        var solutionToReplace = replacedProducts.SplitSolutionWithout(replacedProducts.Volume, replacementTargetIds);

        foreach (var replacement in replaceSolution.Replacements)
        {
            PerformReplacement(solutionToReplace,
                replacement,
                out var outputSolution,
                out var replacementSolution);

            solutionToReplace = outputSolution;
            replacedProducts.AddSolution(replacementSolution, _protoMan);
        }

        Solution finalResultSolution = solutionToReplace;
        finalResultSolution.AddSolution(replacedProducts, _protoMan);
        return finalResultSolution;
    }

    public void PerformReplacement(Solution solution,
        SolutionReplacement replacement,
        out Solution cleanOutput,
        out Solution replacedOutput)
    {
        if (solution.Volume <= 0 || replacement.ReplacementSolution.Volume <= 0)
        {
            cleanOutput = solution;
            replacedOutput = new Solution();
            return;
        }

        Solution solutionToReplace = solution;
        Solution? ignoredSolution = null;

        if (replacement.Whitelist != null)
        {
            solutionToReplace = solution.SplitSolutionWithOnly(solution.Volume, replacement.Whitelist);
            ignoredSolution = solution.SplitSolutionWithout(solution.Volume, replacement.Whitelist);
        }

        var originalVolume = solutionToReplace.Volume;
        solutionToReplace.RemoveSolution(replacement.Amount);

        // Important to make sure this scales with how much is actually replaced.
        // For example, if it replaces 2u of Nutriment with 3u of Gastrotoxin,
        // but there was only 1u left of Nutriment in the original solution,
        // then it should only generate 1.5u of Gastrotoxin.
        var replacedAmount = originalVolume - solutionToReplace.Volume;
        var replacementRatio = replacedAmount / replacement.Amount;
        Solution replacementSolution = replacement.ReplacementSolution.Clone();
        replacementSolution.ScaleSolution(replacementRatio.Float());

        // If there's a whitelist, make sure we readd our non-whitelisted solution back.
        if (ignoredSolution != null && ignoredSolution.Volume > 0)
            solutionToReplace.AddSolution(ignoredSolution, _protoMan);

        cleanOutput = solutionToReplace;
        replacedOutput = replacementSolution;
    }
}
