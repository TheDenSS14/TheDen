using Content.Shared._Floof.Consent;
using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Consent;


/// <summary>
/// This is a prototype for tracking consent categories
/// </summary>
[Prototype("consentCategory")]
public sealed partial class ConsentCategoryPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public int Priority { get; set; } = 100;

    [DataField]
    public HashSet<ProtoId<ConsentTogglePrototype>> Members { get; set; } = new();
}
