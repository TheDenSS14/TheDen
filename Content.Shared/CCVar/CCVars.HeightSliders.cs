// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Whether height & width sliders adjust a character's Fixture Component
    /// </summary>
    public static readonly CVarDef<bool> HeightAdjustModifiesHitbox =
        CVarDef.Create("heightadjust.modifies_hitbox", true, CVar.SERVERONLY);

    /// <summary>
    ///     Whether height & width sliders adjust a player's max view distance
    /// </summary>
    public static readonly CVarDef<bool> HeightAdjustModifiesZoom =
        CVarDef.Create("heightadjust.modifies_zoom", false, CVar.SERVERONLY);

    /// <summary>
    ///     Whether height & width sliders adjust a player's bloodstream volume.
    /// </summary>
    /// <remarks>
    ///     This can be configured more precisely by modifying BloodstreamAffectedByMassComponent.
    /// </remarks>
    public static readonly CVarDef<bool> HeightAdjustModifiesBloodstream =
        CVarDef.Create("heightadjust.modifies_bloodstream", true, CVar.SERVERONLY);
}
