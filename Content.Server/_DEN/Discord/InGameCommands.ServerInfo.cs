// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Discord.DiscordLink;
using Content.Server.Mind;
using NetCord;
using Robust.Server.Player;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord;


/// <summary>
/// This handles discord commands that call game code.
/// </summary>
public sealed partial class InGameCommands
{
    private void OnAdminListCommandRun(CommandReceivedEventArgs args)
    {
        if (args.Message.Author is not GuildUser guildUser
            || args.Message.Guild == null
            || args.Message.Channel == null
            || (guildUser.GetPermissions(args.Message.Guild) & Permissions.ManageMessages) == 0)
            return;

        var admins = _adminManager.AllAdmins
            .Select(GetAdminListText)
            .Order();

        var title = "**Admin List**";
        var adminCount = 0;
        var adminsListText = string.Empty;

        foreach (var admin in admins)
        {
            adminsListText += $"- {admin}\n";
            adminCount++;
        }

        title += $"\nTotal Admins: {adminCount}\n";
        args.Message.Channel.SendMessageAsync(title + adminsListText);
    }

    private void OnCharactersCommandRun(CommandReceivedEventArgs args)
    {
        var sessions = _playerManager.Sessions;
        var characters = sessions
            .Where(session => session.AttachedEntity is { Valid: true })
            .OrderBy(session => MetaData(session.AttachedEntity!.Value).EntityName)
            .Select(GetPlayerListText);

        if (args.Message.Author is not GuildUser guildUser
            || args.Message.Guild == null
            || args.Message.Channel == null)
            return;

        if ((guildUser.GetPermissions(args.Message.Guild) & Permissions.ManageMessages) == 0)
            return;

        var title = "**Character List**";
        var characterCount = 0;
        var charactersListText = string.Empty;

        foreach (var character in characters)
        {
            charactersListText += $"- {character}\n";
            characterCount++;
        }

        title += $"\nTotal Characters: {characterCount}\n";
        args.Message.Channel.SendMessageAsync(title + charactersListText);
    }

    private void OnPlayersCommandRun(CommandReceivedEventArgs args)
    {
        var sessions = _playerManager.Sessions;
        var players = sessions
            .OrderBy(session => session.Data.UserName)
            .Select(GetPlayerListText);

        if (args.Message.Author is not GuildUser guildUser
            || args.Message.Guild == null
            || args.Message.Channel == null)
            return;

        if ((guildUser.GetPermissions(args.Message.Guild) & Permissions.ManageMessages) == 0)
            return;

        var title = "**Player List**";
        var playerCount = 0;
        var playersListText = string.Empty;

        foreach (var player in players)
        {
            playersListText += $"- {player}\n";
            playerCount++;
        }

        title += $"\nTotal Characters: {playerCount}\n";
        args.Message.Channel.SendMessageAsync(title + playersListText);
    }
}
