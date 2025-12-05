using Content.Shared.FixedPoint;
using Content.Shared.Store;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._DEN.Skia;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SkiaComponent : Component
{
    /// <summary>
    /// The currency Skia gain from reaping targets
    /// </summary>
    [DataField("soulSilkCurrencyPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<CurrencyPrototype>))]
    public string SoulSilkCurrencyPrototype = "SoulSilk";

    /// <summary>
    /// How long the Skia takes to "reap" it's target after they are incapacitated, completing this event kills the target
    /// </summary>
    [DataField("reapDuration")]
    public float ReapDuration = 1.5f;

    [DataField("shopActionId")]
    public EntProtoId ShopActionId = "ActionSkiaShop";

    [DataField, AutoNetworkedField]
    public EntityUid? ShopAction;
}
