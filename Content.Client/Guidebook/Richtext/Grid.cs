// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 John Willis <143434770+CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Guidebook.Richtext;

public sealed class Grid : GridContainer, IDocumentTag
{
    public bool TryParseTag(Dictionary<string, string> args, [NotNullWhen(true)] out Control? control)
    {
        HorizontalExpand = true;
        control = this;

        if (args.TryGetValue("Margin", out var margin))
            Margin = new Thickness(float.Parse(margin));

        if (args.TryGetValue("HorizontalAlignment", out var halign))
            HorizontalAlignment = Enum.Parse<HAlignment>(halign);
        else
            HorizontalAlignment = HAlignment.Center;

        if (args.TryGetValue("VerticalAlignment", out var valign))
            VerticalAlignment = Enum.Parse<VAlignment>(valign);

        // Grid properties

        if (args.TryGetValue("Columns", out var columns))
            Columns = int.Parse(columns);

        if (args.TryGetValue("Rows", out var rows))
            Rows = int.Parse(rows);

        return true;
    }
}
