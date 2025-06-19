// SPDX-FileCopyrightText: 2021 Clyybber <darkmine956@gmail.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 ShadowCommander <10494922+ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Administration.Events;
using Content.Shared.GameTicking;
using Robust.Shared.Network;

namespace Content.Client.Administration.Systems
{
    public sealed partial class AdminSystem : EntitySystem
    {
        public event Action<List<PlayerInfo>>? PlayerListChanged;

        public Dictionary<NetUserId, PlayerInfo> PlayerInfos = new();
        public IReadOnlyList<PlayerInfo> PlayerList =>
            PlayerInfos != null ? PlayerInfos.Values.ToList() : new List<PlayerInfo>();

        public override void Initialize()
        {
            base.Initialize();

            InitializeOverlay();
            SubscribeNetworkEvent<FullPlayerListEvent>(OnPlayerListChanged);
            SubscribeNetworkEvent<PlayerInfoChangedEvent>(OnPlayerInfoChanged);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            ShutdownOverlay();
        }

        private void OnPlayerInfoChanged(PlayerInfoChangedEvent ev)
        {
            if(ev.PlayerInfo == null) return;

            if (PlayerInfos == null) PlayerInfos = new();

            PlayerInfos[ev.PlayerInfo.SessionId] = ev.PlayerInfo;
            PlayerListChanged?.Invoke(PlayerInfos.Values.ToList());
        }

        private void OnPlayerListChanged(FullPlayerListEvent msg)
        {
            PlayerInfos = msg.PlayersInfo.ToDictionary(x => x.SessionId, x => x);
            PlayerListChanged?.Invoke(msg.PlayersInfo);
        }
    }
}
