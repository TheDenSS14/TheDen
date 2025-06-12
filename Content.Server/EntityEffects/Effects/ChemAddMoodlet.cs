using Content.Server.Mood;
using Content.Shared.EntityEffects;
using Content.Shared.Mood;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

/// <summary>
///     Adds a moodlet to an entity.
/// </summary>
[UsedImplicitly]
public sealed partial class ChemAddMoodlet : EntityEffect
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var protoMan = IoCManager.Resolve<IPrototypeManager>();
        return Loc.GetString("reagent-effect-guidebook-add-moodlet",
            // Floof changes - Improve ChemAddMoodlet effect guidebook description
            ("use-effect-name", GuidebookShowEffectName),
            ("mood-effect", Loc.GetString($"{MoodSystem.LocMoodEffectNamePrefix}{MoodPrototype.Id}")),
            ("chance", Probability),
            // Floof changes end
            ("amount", protoMan.Index<MoodEffectPrototype>(MoodPrototype.Id).MoodChange),
            ("timeout", protoMan.Index<MoodEffectPrototype>(MoodPrototype.Id).Timeout));
    }

    /// <summary>
    ///     The mood prototype to be applied to the using entity.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<MoodEffectPrototype> MoodPrototype = default!;

    [DataField] public bool GuidebookShowEffectName = false; // Floof - Improve ChemAddMoodlet effect guidebook description

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs _)
            return;

        var entityManager = IoCManager.Resolve<EntityManager>();
        var ev = new MoodEffectEvent(MoodPrototype);
        entityManager.EventBus.RaiseLocalEvent(args.TargetEntity, ev);
    }
}
