// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Chat.Systems;
using Content.Server.Silicons.Bots;
using Content.Shared.Chat;
using Content.Shared.Interaction;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Silicons.Bots;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class WeldbotWeldOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    private ChatSystem _chat = default!;
    private WeldbotSystem _weldbot = default!;
    private SharedInteractionSystem _interaction = default!;
    private ItemToggleSystem _toggle = default!;

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
        _chat = sysManager.GetEntitySystem<ChatSystem>();
        _weldbot = sysManager.GetEntitySystem<WeldbotSystem>();
        _interaction = sysManager.GetEntitySystem<SharedInteractionSystem>();
        _toggle = sysManager.GetEntitySystem<ItemToggleSystem>();
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
            || !_entMan.TryGetComponent<WeldbotComponent>(owner, out var botComp)
            || !_entMan.TryGetComponent<TransformComponent>(target, out var targetXform))
            return HTNOperatorStatus.Failed;

        var weldbot = new Entity<WeldbotComponent>(owner, botComp);

        if (!_weldbot.TryGetWelder(weldbot, out var welder)
            || !_entMan.TryGetComponent<ItemToggleComponent>(welder.Value.Owner, out var toggle)
            || !_weldbot.CanWeldEntity(weldbot, target))
            return HTNOperatorStatus.Failed;

        if (!welder.Value.Comp.Enabled)
            _toggle.TrySetActive((welder.Value.Owner, toggle), true, owner);

        _chat.TrySendInGameICMessage(weldbot.Owner,
            Loc.GetString("weldbot-start-weld"),
            InGameICChatType.Speak,
            hideChat: true,
            hideLog: true);

        if (botComp.IsEmagged)
        {
            if (!_weldbot.CanWeldMob(weldbot, target))
                return HTNOperatorStatus.Failed;
        }
        else
            _interaction.InteractUsing(owner,
                welder.Value.Owner,
                target,
                targetXform.Coordinates);

        return HTNOperatorStatus.Finished;
    }
}
