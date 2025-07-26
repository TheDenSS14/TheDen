using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;


namespace Content.Shared.Consent;


/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ConsentComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<ConsentTogglePrototype>> Consents { get; set; } = new();
}
