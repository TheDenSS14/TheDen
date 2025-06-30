// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Floofstation.Hypno;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;
using Robust.Client.Player;
using Content.Client.Overlays;

namespace Content.Client.Floofstation;

public sealed class HypnotizedSystem : EquipmentHudSystem<HypnotizedComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PsionicHypnoComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, PsionicHypnoComponent component, ref GetStatusIconsEvent args)
    {
        if (!IsActive)
            return;

        if (_playerManager.LocalEntity is not { Valid: true } player
            || !TryComp<HypnotizedComponent>(player, out var hypnoComp)
            || hypnoComp.Master != uid)
            return;

        if (_prototype.TryIndex(component.MasterIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }
}
