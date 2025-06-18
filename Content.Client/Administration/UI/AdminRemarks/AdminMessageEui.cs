// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Riggle <27156122+RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Eui;
using Content.Shared.Administration.Notes;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using static Content.Shared.Administration.Notes.AdminMessageEuiMsg;

namespace Content.Client.Administration.UI.AdminRemarks;

[UsedImplicitly]
public sealed class AdminMessageEui : BaseEui
{
    private readonly AdminMessagePopupWindow _popup;

    public AdminMessageEui()
    {
        _popup = new AdminMessagePopupWindow();
        _popup.OnAcceptPressed += () => SendMessage(new Dismiss(true));
        _popup.OnDismissPressed += () => SendMessage(new Dismiss(false));
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not AdminMessageEuiState s)
        {
            return;
        }

        _popup.SetState(s);
    }

    public override void Opened()
    {
        _popup.UserInterfaceManager.WindowRoot.AddChild(_popup);
        LayoutContainer.SetAnchorPreset(_popup, LayoutContainer.LayoutPreset.Wide);
    }

    public override void Closed()
    {
        _popup.Orphan();
    }
}
