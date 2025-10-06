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
    private string GetAdminListText(ICommonSession session)
    {
        var adminned = _adminManager.IsAdmin(session) ? "Adminned" : "Deadminned";
        var username = session.Data.UserName;

        return username + " (" + adminned + ")";
    }

    private string GetPlayerListText(ICommonSession session)
    {
        if (session.AttachedEntity is not { Valid: true } attachedEntity)
            return $"(IN LOBBY) {session.Data.UserName}";

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
