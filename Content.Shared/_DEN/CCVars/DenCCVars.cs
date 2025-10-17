// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Configuration;

namespace Content.Shared._DEN.CCVars;

[CVarDefs]
public sealed class DenCCVars
{
    /// <summary>
    /// The maximum width of the examine tooltip.
    /// </summary>
    public static readonly CVarDef<float> ExamineTooltipWidth =
        CVarDef.Create("ui.examine_tooltip_width", 400.0f, CVar.CLIENTONLY | CVar.ARCHIVE);
}
