// SPDX-FileCopyrightText: 2024 Kot <1192090+koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Power.Components;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Buckle.Components;
using Content.Shared.Power;

namespace Content.Server.Buckle.Systems;

public sealed class AntiRotOnBuckleSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BuckleComponent, IsRottingEvent>(OnIsRotting);
        SubscribeLocalEvent<AntiRotOnBuckleComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnIsRotting(EntityUid uid, BuckleComponent buckle, ref IsRottingEvent args)
    {
        if (args.Handled)
            return;
        args.Handled = buckle is { Buckled: true, BuckledTo: not null } &&
                       TryComp<AntiRotOnBuckleComponent>(buckle.BuckledTo.Value, out var antiRot) &&
                       antiRot.Enabled;
    }

    private void OnPowerChanged(EntityUid uid, AntiRotOnBuckleComponent component, ref PowerChangedEvent args)
    {
        component.Enabled = !component.RequiresPower || args.Powered;
    }
}
