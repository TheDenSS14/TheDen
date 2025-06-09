using Content.Server.Mood;
using Content.Shared.EntityEffects;
using Content.Shared.Mood;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions
{
    public sealed partial class MoodCategory : EntityEffectCondition
    {
        /// <summary>
        ///     The mood category to be tested for.
        /// </summary>
        [DataField(required: true)]
        public ProtoId<MoodCategoryPrototype> CategoryId = default!;

        /// <summary>
        ///     A loc string describing the category to be tested for.
        /// </summary>
        [DataField]
        public string? CategoryDescription;

        /// <summary>
        ///     If true, succeeds if the entity does not have the specified category.
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
            bool hasMoodCategory = component.CategorisedEffects.ContainsKey(CategoryId.Id);
            return Inverted ? !hasMoodCategory : hasMoodCategory;
        }

        public override string GuidebookExplanation(IPrototypeManager prototype)
        {
            return Loc.GetString("reagent-effect-condition-guidebook-has-mood-effect",
                ("inverted", Inverted),
                ("effect", Loc.GetString(CategoryDescription ?? $"{MoodSystem.LocMoodCategoryNamePrefix}{CategoryId.Id}"))
                );
        }
    }
}
