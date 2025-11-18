// SPDX-FileCopyrightText: 2021 DrSmugleaf
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Administration;
using Content.Shared.CCVar.CVarAccess;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

/// <summary>
/// Contains all the CVars used by content.
/// </summary>
/// <remarks>
/// NOTICE FOR FORKS: Put your own CVars in a separate file with a different [CVarDefs] attribute. RT will automatically pick up on it.
/// </remarks>
[CVarDefs]
public sealed partial class CCVars : CVars
{
    // Only debug stuff lives here.

#if DEBUG
    [CVarControl(AdminFlags.Debug)]
    public static readonly CVarDef<string> DebugTestCVar =
        CVarDef.Create("debug.test_cvar", "default", CVar.SERVER);

    [CVarControl(AdminFlags.Debug)]
    public static readonly CVarDef<float> DebugTestCVar2 =
        CVarDef.Create("debug.test_cvar2", 123.42069f, CVar.SERVER);
#endif

    /// <summary>
    /// A simple toggle to test <c>OptionsVisualizerComponent</c>.
    /// </summary>
    public static readonly CVarDef<bool> DebugOptionVisualizerTest =
        CVarDef.Create("debug.option_visualizer_test", false, CVar.CLIENTONLY);

    /// <summary>
    /// Set to true to disable parallel processing in the pow3r solver.
    /// </summary>
    public static readonly CVarDef<bool> DebugPow3rDisableParallel =
        CVarDef.Create("debug.pow3r_disable_parallel", true, CVar.SERVERONLY);

    /// <summary>
    /// Should the clients window show the server hostname in the title?
    /// </summary>
    public static readonly CVarDef<bool> GameHostnameInTitlebar =
        CVarDef.Create("game.hostname_in_titlebar", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// If true, contraband severity can be viewed in the examine menu
    /// </summary>
    public static readonly CVarDef<bool> ContrabandExamine =
        CVarDef.Create("game.contraband_examine", true, CVar.SERVER | CVar.REPLICATED);

}
