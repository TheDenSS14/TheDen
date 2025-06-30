// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls;

public sealed class MonotoneCheckBox : CheckBox
{
    public new const string StyleClassCheckBox = "monotoneCheckBox";
    public new const string StyleClassCheckBoxChecked = "monotoneCheckBoxChecked";

    public MonotoneCheckBox()
    {
        TextureRect.RemoveStyleClass(CheckBox.StyleClassCheckBox);
        TextureRect.AddStyleClass(StyleClassCheckBox);
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();

        if (TextureRect == null)
            return;

        // Update appearance
        if (Pressed)
            TextureRect.AddStyleClass(StyleClassCheckBoxChecked);
        else
            TextureRect.RemoveStyleClass(StyleClassCheckBoxChecked);

        // Appearance modulations
        Modulate = Disabled ? Color.Gray : Color.White;
    }
}