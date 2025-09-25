// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Station.Components;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Commands;

/// <summary>
/// Helper functions for programming console command completions.
/// </summary>
public static class ContentCompletionHelper
{
    /// <summary>
    /// Return all stations, with their ID as value and name as hint.
    /// </summary>
    public static IEnumerable<CompletionOption> StationIds(IEntityManager entityManager)
    {
        var query = entityManager.EntityQueryEnumerator<StationDataComponent, MetaDataComponent>();
        while (query.MoveNext(out var uid, out _, out var metaData))
        {
            yield return new CompletionOption(uid.ToString(), metaData.EntityName);
        }
    }

    public static IEnumerable<CompletionOption> PlayerAttachedEntities(
        IEntityManager entityManager,
        IPlayerManager playerManager)
    {
        foreach (var player in playerManager.Sessions)
        {
            if (player.AttachedEntity == null)
                continue;

            if (!entityManager.EntityExists(player.AttachedEntity))
                continue;

            yield return new(
                player.AttachedEntity.Value.ToString(),
                player.Data.UserName);
        }
    }
}
