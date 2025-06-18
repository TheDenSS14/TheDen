// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 PrPleGoo <prplegoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon.Components;

namespace Content.Client.Overlays;

public sealed class ShowThirstIconsSystem : EquipmentHudSystem<ShowThirstIconsComponent>
{
    [Dependency] private readonly ThirstSystem _thirst = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThirstComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, ThirstComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive)
            return;

        if (_thirst.TryGetStatusIconPrototype(component, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype!);
    }
}
