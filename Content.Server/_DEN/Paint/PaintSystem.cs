// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Paint;
using Content.Shared.Examine;
using Robust.Shared.ColorNaming;
using Content.Shared.Verbs;
using Content.Server.Administration.Systems;
using Content.Shared.Database;
using Robust.Shared.Utility;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;

namespace Content.Server.Paint;

public sealed partial class PaintSystem
{
    [Dependency] private readonly IAdminManager _adminManager = default!;

    private void OnPaintedComponentStartup(Entity<PaintedComponent> ent, ref ComponentStartup args)
    {
        _appearanceSystem.SetData(ent, PaintVisuals.Painted, true);
    }

    private void OnPaintedComponentShutdown(Entity<PaintedComponent> ent, ref ComponentShutdown args)
    {
        _appearanceSystem.SetData(ent, PaintVisuals.Painted, false);
    }

    private void OnPaintedGetVerbs(Entity<PaintedComponent> ent, ref GetVerbsEvent<Verb> args)
    {
        if (!_adminManager.HasAdminFlag(args.User, AdminFlags.Admin))
            return;

        var removePaint = new Verb
        {
            Text = "Remove Paint",
            Category = VerbCategory.Tricks,
            Impact = LogImpact.Low,
            Priority = (int) AdminVerbSystem.TricksVerbPriorities.RemovePaint,
            Icon = new SpriteSpecifier.Rsi(new ResPath("/Textures/Objects/Specific/Janitorial/rag.rsi"), "rag"),
            Act = () =>
            {
                RemComp<PaintedComponent>(ent);
            }
        };

        var refreshPaint = new Verb
        {
            Text = "Refresh Paint",
            Category = VerbCategory.Tricks,
            Impact = LogImpact.Low,
            Priority = (int) AdminVerbSystem.TricksVerbPriorities.RefreshPaint,
            Icon = new SpriteSpecifier.Rsi(new ResPath("/Textures/Objects/Fun/spraycans.rsi"), "rainbow2_cap"),
            Act = () =>
            {
                _appearanceSystem.SetData(ent, PaintVisuals.Painted, false);
                _appearanceSystem.SetData(ent, PaintVisuals.Painted, true);
            }
        };

        args.Verbs.Add(removePaint);
        args.Verbs.Add(refreshPaint);
    }

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
