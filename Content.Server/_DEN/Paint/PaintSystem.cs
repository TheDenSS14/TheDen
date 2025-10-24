// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Paint;
using Content.Shared.Examine;
using Robust.Shared.ColorNaming;

namespace Content.Server.Paint;

public sealed partial class PaintSystem
{
    private void OnExamined(Entity<PaintComponent> ent, ref ExaminedEvent args)
    {
        var colorName = ColorNaming.Describe(ent.Comp.Color, Loc);
        var colorText = Loc.GetString("paint-component-color-text",
            ("hex", ent.Comp.Color.ToHex()),
            ("name", colorName));

        var examineText = Loc.GetString("paint-component-examine-text", ("color", colorText));
        args.PushMarkup(examineText);
    }

    public void SetColor(Entity<PaintComponent> ent, Color color)
    {
        ent.Comp.Color = color;
        Dirty(ent);
    }
}
