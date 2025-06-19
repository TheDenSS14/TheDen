// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared._DV.CCVars;

/// <summary>
/// DeltaV specific cvars.
/// </summary>
[CVarDefs]
// ReSharper disable once InconsistentNaming - Shush you
public sealed class DCCVars
{
    // Den - Removed all unnecessary CCVars
    /// <summary>
    /// A string containing a list of newline-separated strings to be highlighted in the chat.
    /// </summary>
    public static readonly CVarDef<string> ChatHighlights =
        CVarDef.Create("deltav.chat.highlights",
            "",
            CVar.CLIENTONLY | CVar.ARCHIVE,
            "A list of newline-separated strings to be highlighted in the chat.");

    /// <summary>
    /// An option to toggle the automatic filling of the highlights with the character's info, if available.
    /// </summary>
    public static readonly CVarDef<bool> ChatAutoFillHighlights =
        CVarDef.Create("deltav.chat.auto_fill_highlights",
            false,
            CVar.CLIENTONLY | CVar.ARCHIVE,
            "Toggles automatically filling the highlights with the character's information.");

    /// <summary>
    /// The color in which the highlights will be displayed.
    /// </summary>
    public static readonly CVarDef<string> ChatHighlightsColor =
        CVarDef.Create("deltav.chat.highlights_color",
            "#17FFC1FF",
            CVar.CLIENTONLY | CVar.ARCHIVE,
            "The color in which the highlights will be displayed.");

    /// <summary>
    /// Anti-EORG measure. Will add pacified to all players upon round end.
    /// Its not perfect, but gets the job done.
    /// </summary>
    public static readonly CVarDef<bool> RoundEndPacifist =
        CVarDef.Create("game.round_end_pacifist", false, CVar.SERVERONLY);

    /*
     * No EORG
     */

    /// <summary>
    /// Whether the no EORG popup is enabled.
    /// </summary>
    public static readonly CVarDef<bool> RoundEndNoEorgPopup =
        CVarDef.Create("game.round_end_eorg_popup_enabled", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Skip the no EORG popup.
    /// </summary>
    public static readonly CVarDef<bool> SkipRoundEndNoEorgPopup =
        CVarDef.Create("game.skip_round_end_eorg_popup", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// How long to display the EORG popup for.
    /// </summary>
    public static readonly CVarDef<float> RoundEndNoEorgPopupTime =
        CVarDef.Create("game.round_end_eorg_popup_time", 5f, CVar.SERVER | CVar.REPLICATED);

    /*
     * Auto ACO
     */

    /// <summary>
    /// How long with no captain before requesting an ACO be elected.
    /// </summary>
    public static readonly CVarDef<TimeSpan> RequestAcoDelay =
        CVarDef.Create("game.request_aco_delay", TimeSpan.FromMinutes(15), CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    /// Determines whether an ACO should be requested when the captain leaves during the round,
    /// in addition to cases where there are no captains at round start.
    /// </summary>
    public static readonly CVarDef<bool> RequestAcoOnCaptainDeparture =
        CVarDef.Create("game.request_aco_on_captain_departure", true, CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    /// Determines whether All Access (AA) should be automatically unlocked if no captain is present.
    /// </summary>
    public static readonly CVarDef<bool> AutoUnlockAllAccessEnabled =
        CVarDef.Create("game.auto_unlock_aa_enabled", true, CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    /// How long after an ACO request announcement is made before All Access (AA) should be unlocked.
    /// </summary>
    public static readonly CVarDef<TimeSpan> AutoUnlockAllAccessDelay =
        CVarDef.Create("game.auto_unlock_aa_delay", TimeSpan.FromMinutes(5), CVar.SERVERONLY | CVar.ARCHIVE);

    /*
     * Misc.
     */

    /// <summary>
    /// Whether the Shipyard is enabled.
    /// </summary>
    public static readonly CVarDef<bool> Shipyard =
        CVarDef.Create("shuttle.shipyard", true, CVar.SERVERONLY);

    /// <summary>
    ///    Maximum number of characters in objective summaries.
    /// </summary>
    public static readonly CVarDef<int> MaxObjectiveSummaryLength =
        CVarDef.Create("game.max_objective_summary_length", 256, CVar.SERVER | CVar.REPLICATED);
}
