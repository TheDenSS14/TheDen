// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.SurveillanceCamera;
using Robust.Client.GameObjects;

namespace Content.Client.SurveillanceCamera.UI;

public sealed class SurveillanceCameraSetupBoundUi : BoundUserInterface
{
    [ViewVariables]
    private readonly SurveillanceCameraSetupUiKey _type;

    [ViewVariables]
    private SurveillanceCameraSetupWindow? _window;

    public SurveillanceCameraSetupBoundUi(EntityUid component, Enum uiKey) : base(component, uiKey)
    {
        if (uiKey is not SurveillanceCameraSetupUiKey key)
            return;

        _type = key;
    }

    protected override void Open()
    {
        base.Open();

        _window = new();

        if (_type == SurveillanceCameraSetupUiKey.Router)
        {
            _window.HideNameSelector();
        }

        _window.OpenCentered();
        _window.OnNameConfirm += SendDeviceName;
        _window.OnNetworkConfirm += SendSelectedNetwork;

    }

    private void SendSelectedNetwork(int idx)
    {
        SendMessage(new SurveillanceCameraSetupSetNetwork(idx));
    }

    private void SendDeviceName(string name)
    {
        SendMessage(new SurveillanceCameraSetupSetName(name));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window == null || state is not SurveillanceCameraSetupBoundUiState cast)
        {
            return;
        }

        _window.UpdateState(cast.Name, cast.NameDisabled, cast.NetworkDisabled);
        _window.LoadAvailableNetworks(cast.Network, cast.Networks);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _window!.Dispose();
        }
    }
}
