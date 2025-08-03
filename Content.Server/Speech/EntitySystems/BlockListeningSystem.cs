// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class BlockListeningSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlockListeningComponent, ListenAttemptEvent>(OnListenAttempt);
    }

    private void OnListenAttempt(EntityUid uid, BlockListeningComponent component, ListenAttemptEvent args)
    {
        args.Cancel();
    }
}
