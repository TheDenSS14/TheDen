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
public sealed class InGameCommands : EntitySystem
{
    [Dependency] private readonly AdminSystem _adminSystem = default!;
    [Dependency] private readonly DiscordLink _discordLink = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;
    [Dependency] private readonly IAdminManager _adminManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        _discordLink.RegisterCommandCallback(OnAdminListCommandRun, "adminwho");
        _discordLink.RegisterCommandCallback(OnCharactersCommandRun, "characters");
    }

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
        var characters = sessions.Select(GetCharacterListText);

        if (args.Message.Author is not GuildUser guildUser
            || args.Message.Guild == null
            || args.Message.Channel == null
            || (guildUser.GetPermissions(args.Message.Guild) & Permissions.ManageMessages) == 0)
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

    private string GetAdminListText(ICommonSession session)
    {
        var adminned = _adminManager.IsAdmin(session) ? "Adminned" : "Deadminned";
        var username = session.Data.UserName;

        return username + " (" + adminned + ")";
    }

    private string GetCharacterListText(ICommonSession session)
    {
        if (session.AttachedEntity is not { Valid: true } attachedEntity)
            return string.Empty;

        _mindSystem.TryGetMind(session, out _, out var mind);

        var cachedPlayerInfo = mind != null && mind.UserId != null ? _adminSystem.GetCachedPlayerInfo(mind.UserId.Value) : null;
        var antag = mind?.UserId != null && (cachedPlayerInfo?.Antag ?? false);

        var isAdmin = _adminManager.IsAdmin(session, true);
        var isCurrentlyAdminned = _adminManager.IsAdmin(session) ? " (Adminned)" : " (Deadminned)";

        var antagText = antag ? "(ANTAG) " : string.Empty;
        var name = MetaData(attachedEntity).EntityName + ", " + session.Data.UserName + " ";
        var adminText = isAdmin ? isCurrentlyAdminned : string.Empty;

        return antagText + name + adminText;
    }
}
