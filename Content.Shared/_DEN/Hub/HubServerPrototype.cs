using Robust.Shared.Prototypes;


namespace Content.Shared._DEN.Hub;


/// <summary>
/// This is a prototype for declaring new servers to put on the in-game hub menu.
/// </summary>
[Prototype]
public sealed partial class HubServerPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField(required: true)]
    public string ConnectionString { get; } = string.Empty;

    [DataField(required: true)]
    public LocId DisplayName { get; }
}
