// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 MajorMoth
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<float> GhostRespawnTime =
        CVarDef.Create("ghost.respawn_time", 15f, CVar.SERVERONLY);

    public static readonly CVarDef<int> GhostRespawnMaxPlayers =
        CVarDef.Create("ghost.respawn_max_players", 40, CVar.SERVERONLY);

    public static readonly CVarDef<bool> GhostAllowSameCharacter =
        CVarDef.Create("ghost.allow_same_character", false, CVar.SERVERONLY);

    // DEN edit start

    /// <summary>
    /// This is a configurable grace period which begins ticking down from they moment they spawn, that allows players to quickly respawn if they cryo again.
    /// Unit is minutes.
    /// </summary>
    public static readonly CVarDef<float> RoundjoinRespawnPeriod =
        CVarDef.Create("ghost.roundjoin_respawn_period", 10f, CVar.SERVERONLY);

    // DEN edit end
}
