// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

// ReSharper disable once CheckNamespace
namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Should the Lavaland roundstart generation be enabled.
    /// </summary>
    public static readonly CVarDef<bool> LavalandEnabled =
        CVarDef.Create("lavaland.enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<bool> AllowDuplicatePkaModules =
        CVarDef.Create("modkit.dupes_enabled", true, CVar.REPLICATED | CVar.SERVER);
	

	/// Grasslands Update
	
	/// Sets the maximum number of mining planets to be loaded per shift
    public static readonly CVarDef<int> LavalandMaxLands =
        CVarDef.Create("lavaland.max_lands", 1, CVar.REPLICATED | CVar.SERVER);
	
	/// Forces only a specific map to load. Set to "" to return to random.
    public static readonly CVarDef<string> LavalandForceMap =
        CVarDef.Create("lavaland.force_map", "", CVar.REPLICATED | CVar.SERVER);
}
