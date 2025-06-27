// SPDX-FileCopyrightText: 2025 Baptr0b0t
// SPDX-FileCopyrightText: 2025 GoobBot
// SPDX-FileCopyrightText: 2025 Ted Lukin
// SPDX-FileCopyrightText: 2025 pheenty
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Chat.Systems;
using Content.Server.Medical.CrewMonitoring;
using Content.Server.Pinpointer;
using Content.Shared.Mobs;
using Content.Shared.Chat;
using Robust.Shared.Utility;

namespace Content.Server.RelayedDeathrattle;

public sealed class RelayedDeathrattleSystem : EntitySystem
{
    [Dependency] private readonly NavMapSystem _navMap = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RelayedDeathrattleComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(EntityUid uid, RelayedDeathrattleComponent comp, MobStateChangedEvent args)
    {
        if (comp.Target == null)
            return;


        bool dead;
        var posText = FormattedMessage.RemoveMarkupOrThrow(_navMap.GetNearestBeaconString(uid));
        if (args is { NewMobState: MobState.Critical, OldMobState: MobState.Alive })
            dead = false;
        else if (args.NewMobState == MobState.Dead)
            dead = true;
        else
            return;

        _chat.TrySendInGameICMessage(comp.Target.Value, Loc.GetString(dead ? comp.DeathMessage : comp.CritMessage, ("user", uid), ("position", posText)), InGameICChatType.Speak, hideChat: false);
    }
}
