using System.Linq;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Discord.DiscordLink;
using Content.Server.Mind;
using Robust.Server.Player;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord.Discord;


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
        var admins = _adminManager.AllAdmins
            .Select(GetAdminListText)
            .Order();

        if (args.Message.Channel == null)
            return;

        var title = "**Admin List**";
        var adminsListText = string.Join("\n- ", admins);
        args.Message.Channel.SendMessageAsync(title + adminsListText);
    }

    private void OnCharactersCommandRun(CommandReceivedEventArgs args)
    {
        var sessions = _playerManager.Sessions;
        var characters = sessions.Select(GetCharacterListText);

        if (args.Message.Channel == null)
            return;

        var title = "**Character List**\n";
        var charactersListText = string.Join("\n- ", characters);
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

        var isAdmin = _adminManager.IsAdmin(session, true);
        var isCurrentlyAdminned = _adminManager.IsAdmin(session) ? " (Adminned)" : " (Deadminned)";
        var adminText = isAdmin ? isCurrentlyAdminned : string.Empty;
        var cachedPlayerInfo = mind != null && mind.UserId != null ? _adminSystem.GetCachedPlayerInfo(mind.UserId.Value) : null;
        var antag = mind?.UserId != null && (cachedPlayerInfo?.Antag ?? false);
        var antagText = antag ? "(ANTAG) " : string.Empty;
        var name = MetaData(attachedEntity).EntityName + ", " + session.Data.UserName + " ";

        return antagText + name + adminText;
    }
}
