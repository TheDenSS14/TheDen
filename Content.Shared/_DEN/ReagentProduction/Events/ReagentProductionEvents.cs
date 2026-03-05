// SPDX-FileCopyrightText: 2026 Alex C
//
// SPDX-License-Identifier: MIT

using Content.Shared._DEN.ReagentProduction.Prototypes;
using Content.Shared.DoAfter;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;


namespace Content.Shared._DEN.ReagentProduction.Events;

public sealed class ReagentProductionEvents
{
    [Serializable, NetSerializable,]
    public sealed class ReagentProductionTypeAdded(ProtoId<ReagentProductionTypePrototype> productionType) : EntityEventArgs
    {
        public ProtoId<ReagentProductionTypePrototype> ProductionType { get; } = productionType;
    }

    [Serializable, NetSerializable,]
    public sealed class ReagentProductionTypeRemoved(ProtoId<ReagentProductionTypePrototype> productionType) : EntityEventArgs
    {
        public ProtoId<ReagentProductionTypePrototype> ProductionType { get; } = productionType;
    }
}

[Serializable, NetSerializable,]
public sealed partial class ReagentProductionFillEvent : DoAfterEvent
{
    public ProtoId<ReagentProductionTypePrototype> ProductionType;

    public ReagentProductionFillEvent( ProtoId<ReagentProductionTypePrototype> productionType)
    {
        ProductionType = productionType;
    }

    public override DoAfterEvent Clone() => this;
}

