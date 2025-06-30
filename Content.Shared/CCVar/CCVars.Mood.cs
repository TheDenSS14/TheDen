// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /*
        * Mood System
        */

    public static readonly CVarDef<bool> MoodEnabled =
#if RELEASE
        CVarDef.Create("mood.enabled", true, CVar.SERVER);
#else
        CVarDef.Create("mood.enabled", false, CVar.SERVER);
#endif

    public static readonly CVarDef<bool> MoodIncreasesSpeed =
        CVarDef.Create("mood.increases_speed", true, CVar.SERVER);

    public static readonly CVarDef<bool> MoodDecreasesSpeed =
        CVarDef.Create("mood.decreases_speed", true, CVar.SERVER);

    public static readonly CVarDef<bool> MoodModifiesThresholds =
        CVarDef.Create("mood.modify_thresholds", false, CVar.SERVER);

    public static readonly CVarDef<bool> MoodVisualEffects =
        CVarDef.Create("mood.visual_effects", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
