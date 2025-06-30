// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using System.Threading.Tasks;
using Content.Server.NPC.Pathfinding;
using Content.Server.Silicons.Bots;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Silicons.Bots;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class PickNearbyWeldableOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    private SharedInteractionSystem _interaction = default!;
    private EntityLookupSystem _lookup = default!;
    private WeldbotSystem _weldbot = default!;
    private PathfindingSystem _pathfinding = default!;
    private ItemToggleSystem _toggle = default!;

    [DataField] public string RangeKey = NPCBlackboard.WeldbotWeldRange;

    /// <summary>
    /// Target entity to weld
    /// </summary>
    [DataField(required: true)]
    public string TargetKey = string.Empty;

    /// <summary>
    /// Target entitycoordinates to move to.
    /// </summary>
    [DataField(required: true)]
    public string TargetMoveKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _interaction = sysManager.GetEntitySystem<SharedInteractionSystem>();
        _lookup = sysManager.GetEntitySystem<EntityLookupSystem>();
        _weldbot = sysManager.GetEntitySystem<WeldbotSystem>();
        _pathfinding = sysManager.GetEntitySystem<PathfindingSystem>();
        _toggle = sysManager.GetEntitySystem<ItemToggleSystem>();
    }

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!blackboard.TryGetValue<float>(RangeKey, out var range, _entManager)
            || !_entManager.TryGetComponent<WeldbotComponent>(owner, out var weldbotComp))
            return (false, null);

        var weldbot = new Entity<WeldbotComponent>(owner, weldbotComp);
        var damageQuery = _entManager.GetEntityQuery<DamageableComponent>();

        if (!_weldbot.TryGetWelder(weldbot, out var welder)
            || !_entManager.TryGetComponent<ItemToggleComponent>(welder.Value.Owner, out var toggle))
            return (false, null);

        if (!_weldbot.HasEnoughFuel(weldbot, welder.Value))
        {
            _toggle.TrySetActive((welder.Value.Owner, toggle), false, owner);
            return (false, null);
        }

        foreach (var target in _lookup.GetEntitiesInRange(owner, range))
        {
            if (!damageQuery.HasComp(target)
                || !_weldbot.CanWeldEntity(weldbot, target))
                continue;

            var pathRange = SharedInteractionSystem.InteractionRange;

            //Needed to make sure it doesn't sometimes stop right outside its interaction range, in case of a mob.
            if (_weldbot.CanWeldMob(weldbot, target))
                pathRange--;

            var path = await _pathfinding.GetPath(owner, target, pathRange, cancelToken);
            if (path.Result == PathResult.NoPath)
                continue;

            // Turn off the welder if target is out of range
            if (!_interaction.InRangeUnobstructed(owner, target))
                _toggle.TrySetActive((welder.Value.Owner, toggle), false, owner);

            return (true, new Dictionary<string, object>()
            {
                {TargetKey, target},
                {TargetMoveKey, _entManager.GetComponent<TransformComponent>(target).Coordinates},
                {NPCBlackboard.PathfindKey, path},
            });
        }

        // Also turn off the welder if there's no valid targets
        _toggle.TrySetActive((welder.Value.Owner, toggle), false, owner);
        return (false, null);
    }
}
