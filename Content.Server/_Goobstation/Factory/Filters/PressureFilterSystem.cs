// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Goobstation.Shared.Factory.Filters;
using Content.Shared._Goobstation.Factory.Filters;


namespace Content.Goobstation.Server.Factory.Filters;

public sealed class PressureFilterSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PressureFilterComponent, AutomationFilterEvent>(OnPressureFilter);
    }

    private void OnPressureFilter(Entity<PressureFilterComponent> ent, ref AutomationFilterEvent args)
    {
        // TODO: replace this shit with InternalAir if it gets refactored
        float pressure = 0f;
        if (TryComp<GasTankComponent>(args.Item, out var tank))
            pressure = tank.Air.Pressure;
        else if (TryComp<GasCanisterComponent>(args.Item, out var can))
            pressure = can.Air.Pressure;
        else
            return; // has to be a gas holder

        args.Allowed = pressure >= ent.Comp.Min && pressure <= ent.Comp.Max;
        args.CouldAllow = true; // pressure can change with a gas canister or if the tank/can valve is opened
    }
}
