// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Player;

namespace Content.Shared.Players.PlayTimeTracking;

public interface ISharedPlaytimeManager
{
    bool TryGetTrackerTimes(ICommonSession id, [NotNullWhen(true)] out Dictionary<string, TimeSpan>? time);

    bool TryGetRoleBans(ICommonSession id, [NotNullWhen(true)] out HashSet<string>? bans);
}
