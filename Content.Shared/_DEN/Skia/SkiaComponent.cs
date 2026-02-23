// SPDX-FileCopyrightText: 2025 Jakumba
//
// SPDX-License-Identifier: AGPL-3.0-or-later

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
    public float ReapDuration = 3f;

    /// <summary>
    /// How much glimmer is generated after a reaping
    /// </summary>
    [DataField("reapGlimmerValue")]
    public float ReapGlimmerValue = 50f;

    /// <summary>
    /// How much currency is gained after a reaping
    /// </summary>
    [DataField("silkGained")]
    public float SilkGained = 1f;

    /// <summary>
    /// How many targets the Skia has reaped
    /// </summary>
    [DataField("reapCount")]
    public float ReapCount = 0f;

    [DataField("shopActionId")]
    public EntProtoId ShopActionId = "ActionSkiaShop";

    [DataField, AutoNetworkedField]
    public EntityUid? ShopAction;

    /// <summary>
    /// ProtoId of mobs to spawn on Reap
    /// </summary>
    public EntProtoId MobReapSpawnProtoId = "MobSpectre";

    /// <summary>
    /// Maximum number of spawns that can be generated from either Reaping or SummonShadowsActionId
    /// </summary>
    public float MaxSpawnAmount = 10f;

    /// <summary>
    /// ProtoId of mobs to spawn on SummonShadowsActionId
    /// </summary>
    [DataField]
    public EntProtoId MobTwistShadowProtoId = "MobLivingShadow";

    /// <summary>
    /// How many mobs are spawned on SummonShadowsActionId
    /// </summary>
    [DataField("mobSpawnAmount")]
    public float MobSpawnAmount = 2f;
}
