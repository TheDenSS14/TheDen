using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.Components;

[RegisterComponent]
public sealed partial class AddComponentAfterInjectionComponent : Component
{
    [DataField("addComponents")]
    public ComponentRegistry ComponentsToAdd = new();
}
