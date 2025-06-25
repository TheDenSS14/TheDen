// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Power.Components;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;

namespace Content.Client.Power.EntitySystems;

public sealed class PowerNetSystem : SharedPowerNetSystem
{
    public override bool IsPoweredCalculate(SharedApcPowerReceiverComponent comp)
    {
        return IsPoweredCalculate((ApcPowerReceiverComponent)comp);
    }

    private bool IsPoweredCalculate(ApcPowerReceiverComponent comp)
    {
        return !comp.PowerDisabled
               && !comp.NeedsPower;
    }
}
