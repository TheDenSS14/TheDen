using Content.Shared.Chat.Prototypes;
using Content.Shared.Humanoid.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Vocal;


/// <summary>
/// This is used for imitating species noises.
/// </summary>
[RegisterComponent]
public sealed partial class AdditionalVocalSoundsComponent : Component
{
    [DataField]
    public ProtoId<EmoteSoundsPrototype> AdditionalSounds { get; set; }
}
