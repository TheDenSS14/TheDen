// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// Enables the automatic voting system.
    #if DEBUG
    public static readonly CVarDef<bool> AutoVoteEnabled =
        CVarDef.Create("vote.autovote_enabled", false, CVar.SERVERONLY);
    #else
    public static readonly CVarDef<bool> AutoVoteEnabled =
        CVarDef.Create("vote.autovote_enabled", true, CVar.SERVERONLY);
    #endif

    /// Automatically starts a map vote when returning to the lobby.
    /// Requires auto voting to be enabled.
    public static readonly CVarDef<bool> MapAutoVoteEnabled =
        CVarDef.Create("vote.map_autovote_enabled", true, CVar.SERVERONLY);

    /// Automatically starts a gamemode vote when returning to the lobby.
    /// Requires auto voting to be enabled.
    public static readonly CVarDef<bool> PresetAutoVoteEnabled =
        CVarDef.Create("vote.preset_autovote_enabled", true, CVar.SERVERONLY);
}
