// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<bool> ConsoleLoginLocal =
        CVarDef.Create("console.loginlocal", true, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Automatically log in the given user as host, equivalent to the <c>promotehost</c> command.
    /// </summary>
    public static readonly CVarDef<string> ConsoleLoginHostUser =
        CVarDef.Create("console.login_host_user", "", CVar.ARCHIVE | CVar.SERVERONLY);
}
