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
    private void UpdatePlayerLink(ulong associatedDiscordId, ulong? newDiscordId)
    {
        Task.Run(async () =>
        {
            await _db.UpdateDiscordLink(associatedDiscordId, newDiscordId);
        });
    }

    private void UpdatePlayerLink(NetUserId userId, ulong? newDiscordId)
    {
        Task.Run(async () =>
        {
            await _db.UpdateDiscordLink(userId, newDiscordId);
        });
    }

    private string StartVerify(ulong userId)
    {
        var code = GetRandomCode(CodeLength);
        var pendingLink = new PendingLink(userId, code);

        _pendingLinks.RemoveWhere(link => link.DiscordUserId == userId);
        _pendingLinks.Add(pendingLink);

        return code;
    }

    private string GetRandomCode(int length)
    {
        var code = string.Empty;
        var random = new Random();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(0, _combinedApplicableCodeSymbols.Length - 1);
            var codeSymbol = _combinedApplicableCodeSymbols[index];
            code += codeSymbol;
        }

        return code;
    }

    private async Task SendDirectMessage(ulong userId, string message)
    {
        if (_discordLink.Client == null)
            return;

        var dm = await _discordLink.Client.Rest.GetDMChannelAsync(userId);
        await dm.SendMessageAsync(message);
    }
}

public record struct PendingLink
{
    public ulong DiscordUserId { get; init; }
    public string Code { get; init; }

    public PendingLink(ulong discordUserId, string code)
    {
        DiscordUserId = discordUserId;
        Code = code;
    }
}

public record struct ActiveDiscordLink(NetUserId UserId, ulong DiscordUserId)
{
    public NetUserId InGameUserId { get; init; } = UserId;
    public ulong DiscordUserId { get; init; } = DiscordUserId;
}
