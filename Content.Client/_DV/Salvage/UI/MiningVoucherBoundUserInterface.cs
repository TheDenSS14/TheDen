// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Salvage;
using Robust.Client.UserInterface;

namespace Content.Client._DV.Salvage.UI;

public sealed class MiningVoucherBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private MiningVoucherMenu? _menu;

    public MiningVoucherBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<MiningVoucherMenu>();
        _menu.SetEntity(Owner);
        _menu.OnSelected += i =>
        {
            SendMessage(new MiningVoucherSelectMessage(i));
            Close();
        };
    }
}
