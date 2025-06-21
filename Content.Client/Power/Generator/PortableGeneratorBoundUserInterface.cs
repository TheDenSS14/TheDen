// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Power.Generator;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client.Power.Generator;

[UsedImplicitly]
public sealed class PortableGeneratorBoundUserInterface : BoundUserInterface
{
    private GeneratorWindow? _window;

    public PortableGeneratorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<GeneratorWindow>();
        _window.SetEntity(Owner);
        _window.OnState += args =>
        {
            if (args)
            {
                Start();
            }
            else
            {
                Stop();
            }
        };

        _window.OnPower += SetTargetPower;
        _window.OnEjectFuel += EjectFuel;
        _window.OnSwitchOutput += SwitchOutput;

        _window.OpenCenteredLeft();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not PortableGeneratorComponentBuiState msg)
            return;

        _window?.Update(msg);
    }

    public void SetTargetPower(int target)
    {
        SendMessage(new PortableGeneratorSetTargetPowerMessage(target));
    }

    public void Start()
    {
        SendMessage(new PortableGeneratorStartMessage());
    }

    public void Stop()
    {
        SendMessage(new PortableGeneratorStopMessage());
    }

    public void SwitchOutput()
    {
        SendMessage(new PortableGeneratorSwitchOutputMessage());
    }

    public void EjectFuel()
    {
        SendMessage(new PortableGeneratorEjectFuelMessage());
    }
}
