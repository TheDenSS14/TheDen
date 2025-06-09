using Content.Server.Mood;
using Content.Shared.EntityEffects;
using Content.Shared.Mood;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions
{
    public sealed partial class MoodEffect : EntityEffectCondition
    {
        /// <summary>
        ///     The mood effect to be tested for.
        /// </summary>
        [DataField(required: true)]
        public ProtoId<MoodEffectPrototype> EffectId = default!;

        /// <summary>
        ///     A loc string describing the effect to be tested for.
        /// </summary>
        [DataField]
        public string? EffectDescription;

        /// <summary>
        ///     If true, succeeds if the entity does not have the specified mood.
        /// </summary>
        [DataField]
        public bool Inverted = false;

        /// <summary>
        ///     If false, succeeds if the entity has no mood prototype.
        /// </summary>
        [DataField]
        public bool RequiresMoods = true;

        public override bool Condition(EntityEffectBaseArgs args)
        {
            if (!args.EntityManager.TryGetComponent<MoodComponent>(args.TargetEntity, out var component))
                return !RequiresMoods;
            bool hasMoodEffect = component.UncategorisedEffects.ContainsKey(EffectId.Id) ||
                component.CategorisedEffects.ContainsValue(EffectId.Id);
            return Inverted ? !hasMoodEffect : hasMoodEffect;
        }

        public override string GuidebookExplanation(IPrototypeManager prototype)
        {
            return Loc.GetString("reagent-effect-condition-guidebook-has-mood-effect",
                ("inverted", Inverted),
                ("effect", Loc.GetString(EffectDescription ?? $"{MoodSystem.LocMoodEffectNamePrefix}{EffectId}"))
                );
        }
    }
}
