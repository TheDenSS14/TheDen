// SPDX-FileCopyrightText: 2024 Angelo Fallaria <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Traits.Assorted;
using Content.Shared.Standing;

namespace Content.Shared.Traits.Assorted.Systems;

public sealed class LayingDownModifierSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LayingDownModifierComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, LayingDownModifierComponent component, ComponentStartup args)
    {
        if (!TryComp<LayingDownComponent>(uid, out var layingDown))
            return;

        layingDown.StandingUpTime *= component.LayingDownCooldownMultiplier;
        layingDown.LyingSpeedModifier *= component.DownedSpeedMultiplierMultiplier;
    }
}
