using System.Linq;
using System.Threading.Tasks;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord;


public sealed partial class DiscordUserLink
{
    public void InitializeGame() {}

    private void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs ev)
    {
        if (ev.NewStatus != SessionStatus.Connected)
            return;

        Task.Run(async () => await SetupPlayerAsync(ev.Session.UserId));
    }

    private async Task SetupPlayerAsync(NetUserId userId)
    {
        var link = await _db.GetDiscordLink(userId);

        if (link is not { } discordId
            || _links.Any(targetLink => targetLink.DiscordUserId == discordId))
            return;

        if (_discordLink.Client != null &&
            _discordLink.Client.Cache.Guilds.TryGetValue(_discordLink.GuildId, out var guild))
        {
            var guildUser = await _discordLink.Client.Rest.GetGuildUserAsync(_discordLink.GuildId, discordId);
            _discordLink.Client.Cache.CacheGuildUser(guildUser);
        }

        _links.Add(new(userId, discordId));
    }
}

[AdminCommand(AdminFlags.None)]
public sealed class VerifyCommand : IConsoleCommand
{
    public string Command => "verify";
    public string Description => "Verify your discord account with your SS14 account.";
    public string Help => "Usage: verify <code>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entityManager = IoCManager.Resolve<IEntityManager>();
        var discordUserLink = entityManager.System<DiscordUserLink>();

        if (args.Length < 1 || shell.Player == null)
            return;

        var success = discordUserLink.TryGameVerify(shell.Player.UserId, args[0]);
        var successText = success ? string.Empty : " not";
        shell.WriteLine($"Your discord account has{successText} been verified.");
    }
}
