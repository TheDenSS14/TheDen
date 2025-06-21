// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.LifeDrainer;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Interactions;

public sealed partial class DrainOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    private LifeDrainerSystem _drainer = default!;
    private EntityQuery<LifeDrainerComponent> _drainerQuery;

    [DataField(required: true)]
    public string DrainKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);

        _drainer = sysManager.GetEntitySystem<LifeDrainerSystem>();
        _drainerQuery = _entMan.GetEntityQuery<LifeDrainerComponent>();
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var target = blackboard.GetValue<EntityUid>(DrainKey);

        if (_entMan.Deleted(target))
            return HTNOperatorStatus.Failed;

        if (!_drainerQuery.TryComp(owner, out var wisp))
            return HTNOperatorStatus.Failed;

        // still draining hold your horses
        if (_drainer.IsDraining(wisp))
            return HTNOperatorStatus.Continuing;

        // not draining and no target set, start to drain
        if (wisp.Target == null)
        {
            return _drainer.TryDrain((owner, wisp), target)
                ? HTNOperatorStatus.Continuing
                : HTNOperatorStatus.Failed;
        }

        // stopped draining, clean up and find another one after
        _drainer.CancelDrain(wisp);
        return HTNOperatorStatus.Finished;
    }
}
