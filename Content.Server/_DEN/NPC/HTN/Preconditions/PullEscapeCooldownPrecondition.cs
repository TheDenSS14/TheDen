// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.NPC;
using Content.Server.NPC.HTN.Preconditions;
using Content.Shared.Movement.Pulling.Systems;

namespace Content.Server._DEN.NPC.HTN.Preconditions;

/// <summary>
/// Checks if attempting to escape from a pull is on cooldown
/// </summary>
public sealed partial class PullEscapeCooldownPrecondition : HTNPrecondition
{
    private PullingSystem _pulling = default!;

    [ViewVariables(VVAccess.ReadWrite)] [DataField("onCooldown")] public bool OnCooldown = true;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _pulling = sysManager.GetEntitySystem<PullingSystem>();
    }

    public override bool IsMet(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        return OnCooldown && _pulling.EscapeOnCooldown(owner) ||
            !OnCooldown && !_pulling.EscapeOnCooldown(owner);
    }
}
