
using System.Runtime.InteropServices;
using Content.Server._DEN.Body.Systems;
using Content.Server.Body.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Utility;

namespace Content.Server.Body.Systems;

public sealed partial class BloodstreamSystem
{
    [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;

    private void OnExamined(Entity<BloodstreamComponent> target, ref ExaminedEvent args)
    {
        if (TryComp<BloodExaminerComponent>(args.Examiner, out var bloodExaminer))
            BloodExaminerExamined((args.Examiner, bloodExaminer), target, ref args);
    }

    private void BloodExaminerExamined(Entity<BloodExaminerComponent> examiner,
        Entity<BloodstreamComponent> target,
        ref ExaminedEvent args)
    {
        if (examiner.Owner == target.Owner)
            return;

        // Blood drinker range.
        if (!_interactionSystem.InRangeUnobstructed(examiner.Owner, target.Owner))
            return;

        if (!_prototypeManager.TryIndex(target.Comp.BloodReagent, out var blood))
            throw new DebugAssertException($"Entity {target.Owner} has invalid blood reagent: "
                + target.Comp.BloodReagent.ToString());

        var bloodName = blood.LocalizedName;
        var bloodSuffix = Loc.GetString(examiner.Comp.BloodSuffix);

        // Blood reagent text is colored.
        var bloodText = Loc.GetString("blood-examiner-component-chemical",
            ("color", blood.SubstanceColor.ToHexNoAlpha()),
            ("blood", blood.LocalizedName));

        // Add "blood" to the end if it doesn't already have it, to make the sentence make sense.
        // E.g. "You can smell her apple juice." -> "You can smell her apple juice blood."
        if (!bloodName.EndsWith(bloodSuffix))
            bloodText = Loc.GetString("blood-examiner-component-examine-not-blood",
                ("chemical", bloodText),
                ("suffix", bloodSuffix));

        var examineText = Loc.GetString(examiner.Comp.ExamineText, ("target", target), ("blood", bloodText));
        args.PushMarkup(examineText);
    }
}
