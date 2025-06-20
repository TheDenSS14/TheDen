// SPDX-FileCopyrightText: 2024 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.UserInterface;
using Content.Client.UserInterface.Fragments;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;

namespace Content.Client._DV.CartridgeLoader.Cartridges;

public sealed partial class StockTradingUi : UIFragment
{
    private StockTradingUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new StockTradingUiFragment();

        _fragment.OnBuyButtonPressed += (company, amount) =>
        {
            SendStockTradingUiMessage(StockTradingUiAction.Buy, company, amount, userInterface);
        };
        _fragment.OnSellButtonPressed += (company, amount) =>
        {
            SendStockTradingUiMessage(StockTradingUiAction.Sell, company, amount, userInterface);
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is StockTradingUiState cast)
        {
            _fragment?.UpdateState(cast);
        }
    }

    private static void SendStockTradingUiMessage(StockTradingUiAction action, int company, int amount, BoundUserInterface userInterface)
    {
        var newsMessage = new StockTradingUiMessageEvent(action, company, amount);
        var message = new CartridgeUiMessage(newsMessage);
        userInterface.SendMessage(message);
    }
}
