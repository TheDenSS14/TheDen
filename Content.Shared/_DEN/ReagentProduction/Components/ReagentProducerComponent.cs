// SPDX-FileCopyrightText: 2026 Alex C
//
// SPDX-License-Identifier: MIT

using Content.Shared._DEN.ReagentProduction.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;


namespace Content.Shared._DEN.ReagentProduction.Components;

[RegisterComponent]
public sealed partial class ReagentProducerComponent : Component
{
    [DataField]
    public List<ProtoId<ReagentProductionTypePrototype>> ProductionTypes = [];

    /// <summary>
    ///     The next time to fill solution
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextUpdate;

    /// <summary>
    ///     The interval between updates.
    /// </summary>
    [DataField]
    public TimeSpan UpdateInterval = TimeSpan.FromSeconds(10);
}
