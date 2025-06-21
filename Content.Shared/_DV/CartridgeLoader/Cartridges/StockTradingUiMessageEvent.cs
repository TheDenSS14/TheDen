// SPDX-FileCopyrightText: 2024 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class StockTradingUiMessageEvent(StockTradingUiAction action, int companyIndex, int amount)
    : CartridgeMessageEvent
{
    public readonly StockTradingUiAction Action = action;
    public readonly int CompanyIndex = companyIndex;
    public readonly int Amount = amount;
}

[Serializable, NetSerializable]
public enum StockTradingUiAction
{
    Buy,
    Sell,
}
