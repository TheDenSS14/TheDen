// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Administration;
using Robust.Shared.Toolshed;

namespace Content.Server.Administration.Toolshed;

[ToolshedCommand, AnyCommand]
public sealed class MarkedCommand : ToolshedCommand
{
    [CommandImplementation]
    public IEnumerable<EntityUid> Marked(IInvocationContext ctx)
    {
        var res = (IEnumerable<EntityUid>?)ctx.ReadVar("marked");
        res ??= Array.Empty<EntityUid>();
        return res;
    }
}
