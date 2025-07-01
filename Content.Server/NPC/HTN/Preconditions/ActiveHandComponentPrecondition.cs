// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Hands.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Returns true if the active hand entity has the specified components.
/// </summary>
public sealed partial class ActiveHandComponentPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("invert")]
    public bool Invert = false;

    [DataField("components", required: true)]
    public ComponentRegistry Components = new();

    public override bool IsMet(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<Hand>(NPCBlackboard.ActiveHand, out var hand, _entManager) || hand.HeldEntity == null)
        {
            return Invert;
        }

        foreach (var comp in Components)
        {
            var hasComp = _entManager.HasComponent(hand.HeldEntity, comp.Value.Component.GetType());

            if (!Invert && !hasComp || Invert && hasComp)
                return false;
        }

        return true;
    }
}
