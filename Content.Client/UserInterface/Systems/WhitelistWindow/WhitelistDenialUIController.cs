// SPDX-FileCopyrightText: 2024 Skye <22365940+Skyedra@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Options.UI;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Console;
using Content.Client.UserInterface.Systems.WhitelistWindow.Controls;

namespace Content.Client.UserInterface.Systems.WhitelistWindow;

[UsedImplicitly]
public sealed class WhitelistDenialUIController : UIController
{
    public override void Initialize()
    {
    }
    private WhitelistDenialWindow _whitelistDenialWindow = default!;

    private void EnsureWindow()
    {
        if (_whitelistDenialWindow is { Disposed: false })
            return;

        _whitelistDenialWindow = UIManager.CreateWindow<WhitelistDenialWindow>();
    }

    public void OpenWindow(string denialMessage)
    {
        EnsureWindow();

        _whitelistDenialWindow.SetDenialMessage(denialMessage);

        _whitelistDenialWindow.OpenCentered();
        _whitelistDenialWindow.MoveToFront();
    }
}
