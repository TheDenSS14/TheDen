// SPDX-FileCopyrightText: 2025 Tanix
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._RMC14.CCVar;

[CVarDefs]
public sealed class RMCCVars : CVars
{
    public static readonly CVarDef<float> RMCEmoteCooldownSeconds =
        CVarDef.Create("rmc.emote_cooldown_seconds", 0.8f, CVar.SERVER | CVar.REPLICATED);
}
