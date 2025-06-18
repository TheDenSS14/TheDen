// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.VendingMachines;
using Robust.Client.UserInterface;

namespace Content.Client._DV.VendingMachines.UI;

public sealed class ShopVendorBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private ShopVendorWindow? _window;

    public ShopVendorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<ShopVendorWindow>();
        _window.SetEntity(Owner);
        _window.OpenCenteredLeft();
        _window.Title = EntMan.GetComponent<MetaDataComponent>(Owner).EntityName;
        _window.OnItemSelected += index => SendMessage(new ShopVendorPurchaseMessage(index));
    }
}
