// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Server.Audio;
using Robust.Shared.Audio;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators;

public sealed partial class PlaySoundOperator : HTNOperator
{
    private AudioSystem _audio = default!;

    [DataField(required: true)]
    public SoundSpecifier? Sound;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);

        _audio = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<AudioSystem>();
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var uid = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        _audio.PlayPvs(Sound, uid);

        return base.Update(blackboard, frameTime);
    }
}
