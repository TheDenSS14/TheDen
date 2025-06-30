// SPDX-FileCopyrightText: 2025 AirFryerBuyOneGetOneFree <airfryerbuyonegetonefree@gmail.com>
// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._Goobstation.Factory.Filters;
using Robust.Client.UserInterface;

namespace Content.Client._Goobstation.Factory.UI;

public sealed class LabelFilterBUI : BoundUserInterface
{
    private LabelFilterWindow? _window;

    public LabelFilterBUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<LabelFilterWindow>();
        _window.SetEntity(Owner);
        _window.OnSetLabel += label => SendPredictedMessage(new LabelFilterSetLabelMessage(label));
    }
}
