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

    /// <summary>
    /// "Server Content ID" allows us to interact with prototypes in different ways between servers,
    /// e.g. Salvation and Eternity. We want this because both servers intend to use the exact same codebase.
    /// We can use this to, for example, disable cargo listings on Eternity.
    /// </summary>
    public static readonly CVarDef<string> ServerContentId =
        CVarDef.Create("server.content_id", string.Empty, CVar.NOTIFY | CVar.REPLICATED | CVar.SERVER);
}
