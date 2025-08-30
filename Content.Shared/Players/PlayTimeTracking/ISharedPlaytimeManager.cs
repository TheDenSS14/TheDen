// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Player;

namespace Content.Shared.Players.PlayTimeTracking;

public interface ISharedPlaytimeManager
{
    bool TryGetTrackerTimes(ICommonSession id, [NotNullWhen(true)] out Dictionary<string, TimeSpan>? time);

    bool TryGetRoleBans(ICommonSession id, [NotNullWhen(true)] out HashSet<string>? bans);
}
