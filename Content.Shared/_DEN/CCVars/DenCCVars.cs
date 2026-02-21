// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Dirius77
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

    /// <summary>
    /// For the custom hub in the lobby, how long should the server wait between refreshes?
    /// Note that this is equivalent to the number of servers listed in the prototypes as HTTP requests.
    /// </summary>
    public static readonly CVarDef<int> LobbyRefreshServersInterval =
        CVarDef.Create("lobby.refresh_servers_interval", 30, CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    ///    Makes the flash effect in the shader a black fade instead of a white one.
    /// </summary>
    public static readonly CVarDef<bool> BlackFlashEffect =
        CVarDef.Create("accessibility.black_flash_effect", false,  CVar.CLIENTONLY | CVar.ARCHIVE); // DEN: Black flash instead of white.

    /// <summary>
    /// Whether the maybe bad performance marking glow animation should be running.
    /// </summary>
    public static readonly CVarDef<bool> MarkingGlowAnimation =
        CVarDef.Create("game.marking_glow_animation", false, CVar.SERVERONLY);

    // Denu

    /// <summary>
    /// Whether the Denu's Auto Formatter starts enabled.
    /// </summary>
    public static readonly CVarDef<bool> AutoFormatterEnabled =
        CVarDef.Create("denu.auto_formatter_enabled", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether to remove asterisks in Denu's auto-formatted message results.
    /// </summary>
    public static readonly CVarDef<bool> RemoveAsterisksFromEmotes =
        CVarDef.Create("denu.remove_asterisks", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether to show whisper/subtles typing range when typing.
    /// </summary>
    public static readonly CVarDef<bool> ShowTypingRange =
        CVarDef.Create("denu.show_typing_range", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
