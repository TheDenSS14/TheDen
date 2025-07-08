// SPDX-FileCopyrightText: 2023 AJCM-git
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
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

            // Any delay between zero and 1/tickrate will be equivalent to 1/tickrate delay.
            // Setting delay to zero makes the draw "continuous"
            float drawRate = comp.DrawRate;
            if (comp.Delay == TimeSpan.Zero)
                drawRate *= frameTime;
            else
            {
                if (Timing.CurTime < comp.NextUpdateTime)
                    continue;
            }

            comp.NextUpdateTime += comp.Delay;

            if (!TryGetBatteryFromSlot(uid, out var batteryEnt, out var battery, slot))
                continue;

            if (_battery.TryUseCharge(batteryEnt.Value, drawRate, battery))
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
