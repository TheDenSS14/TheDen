// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 AJCM-git <60196617+ajcm-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deathride58 <deathride58@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Interaction;
using Content.Shared.CombatMode;
using Content.Shared.Timing;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Interactions;

public sealed partial class InteractWithOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    /// <summary>
    /// Key that contains the target entity.
    /// </summary>
    [DataField(required: true)]
    public string TargetKey = default!;

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (_entManager.TryGetComponent<UseDelayComponent>(owner, out var useDelay) && _entManager.System<UseDelaySystem>().IsDelayed((owner, useDelay)) ||
            !blackboard.TryGetValue<EntityUid>(TargetKey, out var moveTarget, _entManager) ||
            !_entManager.TryGetComponent<TransformComponent>(moveTarget, out var targetXform))
        {
            return HTNOperatorStatus.Continuing;
        }

        if (_entManager.TryGetComponent<CombatModeComponent>(owner, out var combatMode))
        {
            _entManager.System<SharedCombatModeSystem>().SetInCombatMode(owner, false, combatMode);
        }

        _entManager.System<InteractionSystem>().UserInteraction(owner, targetXform.Coordinates, moveTarget);
        return HTNOperatorStatus.Finished;
    }
}
