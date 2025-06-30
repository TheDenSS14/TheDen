// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Whether or not a Material Reclaimer is allowed to eat people when emagged.
    /// </summary>
    public static readonly CVarDef<bool> ReclaimerAllowGibbing =
        CVarDef.Create("reclaimer.allow_gibbing", true, CVar.SERVER);
}
