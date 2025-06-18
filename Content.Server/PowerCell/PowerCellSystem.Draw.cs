// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Power.Components;
using Content.Shared.PowerCell;
using Content.Shared.PowerCell.Components;

namespace Content.Server.PowerCell;

public sealed partial class PowerCellSystem
{
    /*
     * Handles PowerCellDraw
     */

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<PowerCellDrawComponent, PowerCellSlotComponent>();

        while (query.MoveNext(out var uid, out var comp, out var slot))
        {
            if (!comp.Enabled)
                continue;

            if (Timing.CurTime < comp.NextUpdateTime)
                continue;

            comp.NextUpdateTime += comp.Delay;

            if (!TryGetBatteryFromSlot(uid, out var batteryEnt, out var battery, slot))
                continue;

            // TCJ: "Multiplying by frameTime to make this tick-invariant. Otherwise it'll draw 30x to 60x faster than you expect."
            if (_battery.TryUseCharge(batteryEnt.Value, comp.DrawRate * frameTime, battery))
                continue;

            var ev = new PowerCellSlotEmptyEvent();
            RaiseLocalEvent(uid, ref ev);
        }
    }

    private void OnDrawChargeChanged(EntityUid uid, PowerCellDrawComponent component, ref ChargeChangedEvent args)
    {
        // Update the bools for client prediction.
        var canUse = component.UseRate <= 0f || args.Charge > component.UseRate;

        var canDraw = component.DrawRate <= 0f || args.Charge > 0f;

        if (canUse != component.CanUse || canDraw != component.CanDraw)
        {
            component.CanDraw = canDraw;
            component.CanUse = canUse;
            Dirty(uid, component);
        }
    }

    private void OnDrawCellChanged(EntityUid uid, PowerCellDrawComponent component, PowerCellChangedEvent args)
    {
        var canDraw = !args.Ejected && HasCharge(uid, float.MinValue);
        var canUse = !args.Ejected && HasActivatableCharge(uid, component);

        if (!canDraw)
        {
            var ev = new PowerCellSlotEmptyEvent();
            RaiseLocalEvent(uid, ref ev);
        }

        if (canUse != component.CanUse || canDraw != component.CanDraw)
        {
            component.CanDraw = canDraw;
            component.CanUse = canUse;
            Dirty(uid, component);
        }
    }
}
