namespace Content.Shared._DEN.Traits;


[RegisterComponent]
public sealed partial class TenuousGripComponent : Component
{
    [ViewVariables]
    public string UnrevivableMessage { get; set; } = "tenuous-grip-unrevivable";

    [DataField]
    public double ReviveThreshold { get; set; } = 300f;
}
