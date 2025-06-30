// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.UserInterface;
using Content.Client.UserInterface.Fragments;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Shared.CartridgeLoader;

namespace Content.Client.CartridgeLoader.Cartridges;

public sealed partial class GlimmerMonitorUi : UIFragment
{
    private GlimmerMonitorUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new GlimmerMonitorUiFragment();

        _fragment.OnSync += _ => SendSyncMessage(userInterface);
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not GlimmerMonitorUiState monitorState)
            return;

        _fragment?.UpdateState(monitorState.GlimmerValues);
    }

    private void SendSyncMessage(BoundUserInterface userInterface)
    {
        var syncMessage = new GlimmerMonitorSyncMessageEvent();
        var message = new CartridgeUiMessage(syncMessage);
        userInterface.SendMessage(message);
    }
}
