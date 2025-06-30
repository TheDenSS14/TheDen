// SPDX-FileCopyrightText: 2024 zelezniciar1 <39102800+zelezniciar1@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Atmos.Components;

namespace Content.Shared.Atmos.Consoles;

public abstract partial class SharedAtmosAlertsComputerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AtmosAlertsComputerComponent, AtmosAlertsComputerDeviceSilencedMessage>(OnDeviceSilencedMessage);
    }

    private void OnDeviceSilencedMessage(EntityUid uid, AtmosAlertsComputerComponent component, AtmosAlertsComputerDeviceSilencedMessage args)
    {
        if (args.SilenceDevice)
            component.SilencedDevices.Add(args.AtmosDevice);

        else
            component.SilencedDevices.Remove(args.AtmosDevice);

        Dirty(uid, component);
    }
}
