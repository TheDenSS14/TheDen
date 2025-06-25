// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Rejuvenate;

namespace Content.Server.FloofStation;

public sealed class AnomalyJobSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AnomalyJobComponent, ComponentStartup>(OnInit);
    }

    private void OnInit(EntityUid uid, AnomalyJobComponent component, ComponentStartup args)
    {
        RaiseLocalEvent(uid, new RejuvenateEvent());
    }
}
