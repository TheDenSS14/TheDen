namespace Content.Server._DEN.GameTicking.Rules;


/// <summary>
/// This is used for handling the high danger preset.
/// </summary>
[RegisterComponent]
public sealed partial class LowDangerRuleComponent : Component, IFakePreset
{
    [DataField]
    public HashSet<EntityUid> AdditionalGameRules { get; set; } = new();
}
