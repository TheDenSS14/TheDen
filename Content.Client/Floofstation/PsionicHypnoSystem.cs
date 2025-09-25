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

public sealed class PsionicHypnoSystem : EquipmentHudSystem<PsionicHypnoComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HypnotizedComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, HypnotizedComponent component, ref GetStatusIconsEvent args)
    {
        if (!IsActive)
            return;

        if (_playerManager.LocalEntity is not { Valid: true } player
            || !TryComp<PsionicHypnoComponent>(player, out var hypnoComp)
            || !component.Masters.Contains(player))
            return;

        if (_prototype.TryIndex(hypnoComp.SubjectIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }
}
