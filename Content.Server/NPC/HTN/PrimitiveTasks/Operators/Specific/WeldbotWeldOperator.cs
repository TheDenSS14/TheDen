using Content.Server.Chat.Systems;
using Content.Server.Silicons.Bots;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Silicons.Bots;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class WeldbotWeldOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    private WeldbotSystem _weldbot = default!;
    private SharedInteractionSystem _interaction = default!;

    public const string SiliconTag = "SiliconMob";
    public const string WeldotFixableStructureTag = "WeldbotFixableStructure";

    /// <summary>
    /// Target entity to inject.
    /// </summary>
    [DataField(required: true)]
    public string TargetKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _weldbot = sysManager.GetEntitySystem<WeldbotSystem>();
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

        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entMan)
            || _entMan.Deleted(target)
            || !_interaction.InRangeUnobstructed(owner, target)
            || !_entMan.TryGetComponent<WeldbotComponent>(owner, out var botComp))
            return HTNOperatorStatus.Failed;

        var weldbot = new Entity<WeldbotComponent>(owner, botComp);

        if (!_weldbot.CanWeldEntity(weldbot, target)
            || !_weldbot.TryWeldEntity(weldbot, target, true))
            return HTNOperatorStatus.Failed;

        return HTNOperatorStatus.Finished;
    }
}
