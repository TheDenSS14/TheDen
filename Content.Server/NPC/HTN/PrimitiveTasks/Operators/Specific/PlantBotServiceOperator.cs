// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Botany.Components;
using Content.Server.Silicons.Bots;
using Content.Shared.Interaction;
using Content.Shared.Silicons.Bots;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class PlantbotServiceOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    private PlantbotSystem _plantbot = default!;
    private SharedInteractionSystem _interaction = default!;

    /// <summary>
    /// Target entity to inject.
    /// </summary>
    [DataField(required: true)]
    public string TargetKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);

        _plantbot = sysManager.GetEntitySystem<PlantbotSystem>();
        _interaction = sysManager.GetEntitySystem<SharedInteractionSystem>();
    }

    public override void TaskShutdown(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.TaskShutdown(blackboard, status);
        blackboard.Remove<EntityUid>(TargetKey);
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entMan) || _entMan.Deleted(target))
            return HTNOperatorStatus.Failed;

        if (!_entMan.TryGetComponent<PlantbotComponent>(owner, out var botComp)
            || !_entMan.TryGetComponent<PlantHolderComponent>(target, out var plantHolderComponent))
            return HTNOperatorStatus.Failed;

        var bot = new Entity<PlantbotComponent>(owner, botComp);
        var holder = new Entity<PlantHolderComponent>(target, plantHolderComponent);

        if (!_interaction.InRangeUnobstructed(owner, target)
            || !_plantbot.CanServicePlantHolder(bot, holder))
            return HTNOperatorStatus.Failed;

        if (botComp.IsEmagged)
        {
            if (_plantbot.CanDrinkPlant(bot, holder))
                _plantbot.TryDoDrinkPlant(bot, holder);
            else
                return HTNOperatorStatus.Failed;
        }
        else
        {
            if (_plantbot.CanWaterPlantHolder(bot, holder))
                _plantbot.TryDoWaterPlant(bot, holder);
            else if (_plantbot.CanWeedPlantHolder(bot, holder))
                _plantbot.TryDoWeedPlant(bot, holder);
            else
                return HTNOperatorStatus.Failed;
        }

        return HTNOperatorStatus.Finished;
    }
}
