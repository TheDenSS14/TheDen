// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Robust.Shared.Player;

namespace Content.Client.Players.PlayTimeTracking;

public sealed partial class JobRequirementsManager
{
    public bool TryGetRoleBans(ICommonSession id, [NotNullWhen(true)] out HashSet<string>? bans)
    {
        bans = _roleBans.ToHashSet();
        return bans != null;
    }
}
