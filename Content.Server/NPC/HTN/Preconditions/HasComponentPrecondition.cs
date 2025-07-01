// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Hands.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Returns true if the entity has the specified components.
/// </summary>
public sealed partial class HasComponentPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("invert")]
    public bool Invert = false;

    [DataField("components", required: true)]
    public ComponentRegistry Components = new();

    public override bool IsMet(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        foreach (var comp in Components)
        {
            var hasComp = _entManager.HasComponent(owner, comp.Value.Component.GetType());

            if (!Invert && !hasComp || Invert && hasComp)
                return false;
        }

        return true;
    }
}
