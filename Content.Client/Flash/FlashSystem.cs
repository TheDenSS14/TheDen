// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Flash;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.GameStates;
using Robust.Shared.Timing;

namespace Content.Client.Flash
{
    public sealed class FlashSystem : SharedFlashSystem
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IOverlayManager _overlayManager = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<FlashableComponent, ComponentHandleState>(OnFlashableHandleState);
        }

        private void OnFlashableHandleState(EntityUid uid, FlashableComponent component, ref ComponentHandleState args)
        {
            if (args.Current is not FlashableComponentState state)
                return;

            // Yes, this code is awful. I'm just porting it to an entity system so don't blame me.
            if (_playerManager.LocalEntity != uid)
            {
                return;
            }

            if (state.Time == default)
            {
                return;
            }

            // Few things here:
            // 1. If a shorter duration flash is applied then don't do anything
            // 2. If the client-side time is later than when the flash should've ended don't do anything
            var calculatedStateDuration = state.Duration * state.DurationMultiplier;

            var currentTime = _gameTiming.CurTime.TotalSeconds;
            var newEndTime = state.Time.TotalSeconds + calculatedStateDuration;
            var currentEndTime = component.LastFlash.TotalSeconds + component.Duration;

            if (currentEndTime > newEndTime)
            {
                return;
            }

            if (currentTime > newEndTime)
            {
                return;
            }

            component.LastFlash = state.Time;
            component.Duration = calculatedStateDuration;

            var overlay = _overlayManager.GetOverlay<FlashOverlay>();
            overlay.ReceiveFlash(component.Duration);
        }
    }
}
