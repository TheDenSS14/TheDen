// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using System.Threading.Tasks;
using Content.Server.Botany.Components;
using Content.Server.NPC.Pathfinding;
using Content.Server.Silicons.Bots;
using Content.Shared.Interaction;
using Content.Shared.Silicons.Bots;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class PickNearbyServicableHydroponicsTrayOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    private EntityLookupSystem _lookup = default!;
    private PathfindingSystem _pathfinding = default!;
    private PlantbotSystem _plantbot = default!;

    /// <summary>
    /// Determines how close the bot needs to be to service a tray
    /// </summary>
    [DataField] public string RangeKey = NPCBlackboard.PlantbotServiceRange;

    /// <summary>
    /// Target entity to service
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

        _lookup = sysManager.GetEntitySystem<EntityLookupSystem>();
        _pathfinding = sysManager.GetEntitySystem<PathfindingSystem>();
        _plantbot = sysManager.GetEntitySystem<PlantbotSystem>();
    }

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!blackboard.TryGetValue<float>(RangeKey, out var range, _entManager)
            || !_entManager.TryGetComponent<PlantbotComponent>(owner, out var botComp))
            return (false, null);

        var entityQuery = _entManager.GetEntityQuery<PlantHolderComponent>();

        foreach (var target in _lookup.GetEntitiesInRange(owner, range))
        {
            if (!entityQuery.TryGetComponent(target, out var plantHolderComponent))
                continue;

            var bot = new Entity<PlantbotComponent>(owner, botComp);
            var holder = new Entity<PlantHolderComponent>(target, plantHolderComponent);
            if (!_plantbot.CanServicePlantHolder(bot, holder))
                continue;

            //Needed to make sure it doesn't sometimes stop right outside it's interaction range
            var pathRange = SharedInteractionSystem.InteractionRange - 1f;
            var path = await _pathfinding.GetPath(owner, target, pathRange, cancelToken);

            if (path.Result == PathResult.NoPath)
                continue;

            return (true, new Dictionary<string, object>()
            {
                {TargetKey, target},
                {TargetMoveKey, _entManager.GetComponent<TransformComponent>(target).Coordinates},
                {NPCBlackboard.PathfindKey, path},
            });
        }

        return (false, null);
    }
}
