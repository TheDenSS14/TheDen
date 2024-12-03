namespace Content.Shared.Consent;

using Robust.Shared.Prototypes;

/// <summary>
/// TODO
/// </summary>
[Prototype("consentToggle")]
public sealed partial class ConsentTogglePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;
}
