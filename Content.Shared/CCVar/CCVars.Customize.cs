// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Allow players to add extra pronouns to their examine window.
    ///     It looks something like "She also goes by they/them pronouns."
    /// </summary>
    public static readonly CVarDef<bool> AllowCosmeticPronouns =
        CVarDef.Create("customize.allow_cosmetic_pronouns", true, CVar.REPLICATED);

    /// <summary>
    ///     Allow players to set their own Station AI names.
    /// </summary>
    public static readonly CVarDef<bool> AllowCustomStationAiName =
        CVarDef.Create("customize.allow_custom_station_ai_name", true, CVar.REPLICATED);

    /// <summary>
    ///     Allow players to set their own cyborg names. (borgs, mediborgs, etc)
    /// </summary>
    public static readonly CVarDef<bool> AllowCustomCyborgName =
        CVarDef.Create("customize.allow_custom_cyborg_name", true, CVar.REPLICATED);
}
