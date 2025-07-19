// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{

    /// <summary>
    ///     How many Low Danger rounds should there be after a High Danger round?
    /// </summary>
    public static readonly CVarDef<int> LowDangerRoundsAfterHighDanger =
        CVarDef.Create("game.low_danger_rounds_after_high_danger", 1, CVar.SERVER | CVar.ARCHIVE);

    /// <summary>
    /// URL of the Discord webhook which will relay round the round to the events logging channel.
    /// </summary>
    public static readonly CVarDef<string> DiscordNewRoundWebhook =
        CVarDef.Create("discord.new_round_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);
}
