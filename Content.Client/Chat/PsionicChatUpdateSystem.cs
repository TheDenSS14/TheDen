// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Abilities.Psionics;
using Content.Client.Chat.Managers;
using Robust.Client.Player;

namespace Content.Client.Chat
{
    public sealed class PsionicChatUpdateSystem : EntitySystem
    {
        [Dependency] private readonly IChatManager _chatManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<PsionicComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<PsionicComponent, ComponentRemove>(OnRemove);
        }

        public PsionicComponent? Player => CompOrNull<PsionicComponent>(_playerManager.LocalPlayer?.ControlledEntity);
        public bool IsPsionic => Player != null;

        private void OnInit(EntityUid uid, PsionicComponent component, ComponentInit args)
        {
            _chatManager.UpdatePermissions();
        }

        private void OnRemove(EntityUid uid, PsionicComponent component, ComponentRemove args)
        {
            _chatManager.UpdatePermissions();
        }
    }
}
