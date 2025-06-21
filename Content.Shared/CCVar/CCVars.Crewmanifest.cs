// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Setting this allows a crew manifest to be opened from any window
    ///     that has a crew manifest button, and sends the correct message.
    ///     If this is false, only in-game entities will allow you to see
    ///     the crew manifest, if the functionality is coded in.
    ///     Having administrator priveledge ignores this, but will still
    ///     hide the button in UI windows.
    /// </summary>
    public static readonly CVarDef<bool> CrewManifestWithoutEntity =
        CVarDef.Create("crewmanifest.no_entity", true, CVar.REPLICATED);

    /// <summary>
    ///     Setting this allows the crew manifest to be viewed from 'unsecure'
    ///     entities, such as the PDA.
    /// </summary>
    public static readonly CVarDef<bool> CrewManifestUnsecure =
        CVarDef.Create("crewmanifest.unsecure", true, CVar.REPLICATED);
}
