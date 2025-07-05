// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2023 20kdc
// SPDX-FileCopyrightText: 2023 Arendian
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT
using Content.Server.Silicons.Bots;
using Content.Server.Chat.Systems;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.NPC.Components;
using Content.Shared.Chat;
using Content.Shared.Damage;
using Content.Shared.Emag.Components;
using Content.Shared.Interaction;
using Content.Shared.Silicon.Components;
using Content.Shared.Silicons.Bots;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class MedibotInjectOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    private MedibotSystem _medibot = default!;
    private SharedInteractionSystem _interaction = default!;

    /// <summary>
    /// Target entity to inject.
    /// </summary>
    [DataField("targetKey", required: true)]
    public string TargetKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _medibot = sysManager.GetEntitySystem<MedibotSystem>();
        _interaction = sysManager.GetEntitySystem<SharedInteractionSystem>();
    }

    public override void TaskShutdown(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.TaskShutdown(blackboard, status);
        blackboard.Remove<EntityUid>(TargetKey);
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        // TODO: Wat
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entMan) || _entMan.Deleted(target))
            return HTNOperatorStatus.Failed;

        if (_entMan.HasComponent<SiliconComponent>(target))
            return HTNOperatorStatus.Failed;

        if (!_entMan.TryGetComponent<MedibotComponent>(owner, out var botComp))
            return HTNOperatorStatus.Failed;

        if (!_entMan.TryGetComponent<DamageableComponent>(target, out var damage))
            return HTNOperatorStatus.Failed;

        var total = damage.TotalDamage;

        if (!_interaction.InRangeUnobstructed(owner, target))
            return HTNOperatorStatus.Failed;

        // always inject healthy patients when emagged
        if (total == 0 && !_entMan.HasComponent<EmaggedComponent>(owner))
            return HTNOperatorStatus.Failed;

        // FINALLY actually try to perform the injection
        if (!_medibot.TryInjectTarget(owner, target, false, botComp))
            return HTNOperatorStatus.Failed;

        return HTNOperatorStatus.Finished;
    }
}
