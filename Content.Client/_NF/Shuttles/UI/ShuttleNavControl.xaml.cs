// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 neuPanda <chriseparton@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

// NeuPanda - This file is licensed under AGPLv3
// Copyright (c) 2025 NeuPanda
// See AGPLv3.txt for details.
using Content.Shared._NF.Shuttles.Events;
using Content.Shared.Shuttles.BUIStates;
using Robust.Shared.Physics.Components;

namespace Content.Client.Shuttles.UI
{
    public sealed partial class ShuttleNavControl
    {
        public InertiaDampeningMode DampeningMode { get; set; }

        private void NfUpdateState(NavInterfaceState state)
        {

            if (!EntManager.GetCoordinates(state.Coordinates).HasValue ||
                !EntManager.TryGetComponent(EntManager.GetCoordinates(state.Coordinates).GetValueOrDefault().EntityId, out TransformComponent? transform) ||
                !EntManager.TryGetComponent(transform.GridUid, out PhysicsComponent? physicsComponent))
            {
                return;
            }

            DampeningMode = state.DampeningMode;
        }
    }
}
