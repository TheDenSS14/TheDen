using Content.Shared._DEN.Traits.EntitySystems;
using Content.Shared.Mood;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Traits.Components;

[RegisterComponent]
[Access(typeof(SharedNoosphericMoodSystem))]
public sealed partial class NoosphericMoodComponent : Component
{
    [DataField("thresholds")]
    public Dictionary<ProtoId<MoodEffectPrototype>, double> GlimmerThresholds = new();
}
