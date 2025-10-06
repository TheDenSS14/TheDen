using System.Linq;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Discord.DiscordLink;
using Content.Server.Mind;
using Content.Shared.Administration;
using NetCord;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord;

public sealed partial class InGameCommands
{
    private void OnSendMessageCommand(CommandReceivedEventArgs eventArgs)
    {
        if (eventArgs.Message.Author is not GuildUser guildUser
            || eventArgs.Message.Guild == null
            || eventArgs.Message.Channel == null)
            return;

        if (!_userLink.IsInGameAdmin(guildUser.Id))
            return;

        var args = eventArgs.Arguments;

        if (!args.HasArgument(0) || !args.HasArgument(1))
            return;

        var username = args.GetArgument(0);
        var message =  args.GetArgument(1);
        args.TryGetBoolArgument(2, out var playSound);

        if (!_playerManager.TryGetPlayerDataByUsername(username, out var player))
        {
            eventArgs.Message.ReplyAsync("No such player: ``" + username + "``.");
            return;
        }

        // Fake an HTTP bwoink.
        var systemBwoinkUser = new NetUserId(Guid.Empty);
        var adminOnly = message.StartsWith("ao:");

        if (adminOnly && message.Length > 3)
            message = message[3..].Trim();

        _userLink.GetRole(guildUser, out var role);
        _userLink.GetRoleColor(guildUser, out var hex);

        var roleName = role?.Name ?? "Admin";
        hex ??= "#ffa500";

        var bwoinkText = new SharedBwoinkSystem.BwoinkTextMessage(
            player.UserId,
            systemBwoinkUser,
            message,
            DateTime.Now,
            playSound,
            adminOnly);

        var bwoinkActionBody = new ServerApi.BwoinkActionBody
        {
            Guid = player.UserId,
            RoleColor = hex,
            RoleName = roleName,
            Username = guildUser.Nickname ?? guildUser.Username,
            Text = message,
            UserOnly = false,
            WebhookUpdate = true
        };

        _bwoinkSystem.OnWebhookBwoinkTextMessage(bwoinkText, bwoinkActionBody);
    }
}
