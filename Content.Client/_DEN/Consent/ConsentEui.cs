// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client._Floof.Consent.UI.Windows;
using Content.Client.Eui;
using JetBrains.Annotations;

namespace Content.Client._DEN.Consent;

[UsedImplicitly]
public sealed class ConsentEui : BaseEui
{
    private ConsentWindow Window { get; }

    public ConsentEui()
    {
        Window = new ConsentWindow();
    }

    public override void Opened()
    {
        base.Opened();
        Window.OpenCentered();
    }

    public override void Closed()
    {
        base.Closed();
        Window.Close();
    }
}
