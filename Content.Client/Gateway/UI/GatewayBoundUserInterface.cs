// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Gateway;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client.Gateway.UI;

[UsedImplicitly]
public sealed class GatewayBoundUserInterface : BoundUserInterface
{
    private GatewayWindow? _window;

    public GatewayBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<GatewayWindow>();
        _window.SetEntity(EntMan.GetNetEntity(Owner));

        _window.OpenPortal += destination =>
        {
            SendMessage(new GatewayOpenPortalMessage(destination));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not GatewayBoundUserInterfaceState current)
            return;

        _window?.UpdateState(current);
    }
}
