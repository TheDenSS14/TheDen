// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.ReverseEngineering;
using JetBrains.Annotations;
using Robust.Client.GameObjects;

namespace Content.Client.Nyanotrasen.ReverseEngineering;

[UsedImplicitly]
public sealed class ReverseEngineeringMachineBoundUserInterface : BoundUserInterface
{
    private ReverseEngineeringMachineMenu? _revMenu;

    public ReverseEngineeringMachineBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _revMenu = new ReverseEngineeringMachineMenu();

        _revMenu.OnClose += Close;
        _revMenu.OpenCentered();

        _revMenu.OnScanButtonPressed += _ =>
        {
            SendMessage(new ReverseEngineeringMachineScanButtonPressedMessage());
        };

        _revMenu.OnSafetyButtonToggled += safetyArgs =>
        {
            SendMessage(new ReverseEngineeringMachineSafetyButtonToggledMessage(safetyArgs.Pressed));
        };

        _revMenu.OnAutoScanButtonToggled += autoArgs =>
        {
            SendMessage(new ReverseEngineeringMachineAutoScanButtonToggledMessage(autoArgs.Pressed));
        };

        _revMenu.OnStopButtonPressed += _ =>
        {
            SendMessage(new ReverseEngineeringMachineStopButtonPressedMessage());
        };

        _revMenu.OnEjectButtonPressed += _ =>
        {
            SendMessage(new ReverseEngineeringMachineEjectButtonPressedMessage());
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        switch (state)
        {
            case ReverseEngineeringMachineScanUpdateState msg:
                _revMenu?.SetButtonsDisabled(msg);
                _revMenu?.UpdateInformationDisplay(msg);
                _revMenu?.UpdateProbeTickProgressBar(msg);
                break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _revMenu?.Dispose();
    }
}

