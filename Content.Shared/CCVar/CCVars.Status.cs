// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<string> StatusMoMMIUrl =
        CVarDef.Create("status.mommiurl", "", CVar.SERVERONLY);

    public static readonly CVarDef<string> StatusMoMMIPassword =
        CVarDef.Create("status.mommipassword", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

}
