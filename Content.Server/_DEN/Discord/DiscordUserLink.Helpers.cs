using System.Linq;
using System.Threading.Tasks;
using NetCord;
using Robust.Shared.Network;
using Robust.Shared.Utility;
using Color = NetCord.Color;


namespace Content.Server._DEN.Discord;


public sealed partial class DiscordUserLink
{
    private readonly ulong[] _patronRoleIds = new[]
    {
        (ulong) 1340025143065575567, // Executive Officer
        (ulong) 1340024869072670801, // Captain
        (ulong) 1340024824533094411, // Central Commander
        (ulong) 1340024729112940615, // Nanotrasen Executive
        (ulong) 1379878777152213174 // Operations Director
    };

    private readonly ulong[] _staffRoleIds =
    [
        1392313569390886942, // Owner
        1302235013986910219, // Manager
        1302235039651598386, // Head Admin
        1302235089677320245, // Senior Admin
        1302235145889124383, // Admin
        1302235169591394305, // Trial Admin
    ];

    public bool IsPatron(NetUserId userId)
    {
        var link = GetLink(userId);

        if (link is not { } discordLink)
            return false;

        var guildUser = GetDiscordIdAsUser(discordLink.DiscordUserId);

        if (guildUser == null)
            return false;

        var value = guildUser.RoleIds.Any(roleId => _patronRoleIds.Contains(roleId));
        return value;
    }

    public bool IsPatron(GuildUser user, NetUserId userId)
    {
        var value = user.RoleIds.Any(roleId => _patronRoleIds.Contains(roleId));
        return value;
    }

    public bool GetRoleColor(NetUserId userId, out string? hex)
    {
        var link = GetLink(userId);
        hex = null;

        if (link is not { } discordLink)
            return false;

        if (_discordLink.Client == null
            || !_discordLink.Client.Cache.Guilds.TryGetValue(_discordLink.GuildId, out var guild))
            return false;

        var guildUser = GetDiscordIdAsUser(discordLink.DiscordUserId);

        if (guildUser == null)
            return false;

        if (!IsPatron(guildUser, userId))
            return false;

        var roles = guildUser
            .GetRoles(guild)
            .OrderByDescending(r => r.Position);

        hex = (from role in roles where role.Color.RawValue != 0 select role.Color.ToString()).FirstOrDefault();
        return hex != null;
    }

    public bool IsStaff(NetUserId userId)
    {
        var link = GetLink(userId);

        if (link is not { } discordLink)
            return false;

        var guildUser = GetDiscordIdAsUser(discordLink.DiscordUserId);

        if (guildUser == null)
            return false;

        var value = guildUser.RoleIds.Any(roleId => _patronRoleIds.Contains(roleId));
        return value;
    }

    public int? GetAdminRankOfRole(NetUserId userId)
    {
        var link = GetLink(userId);

        if (link is not { } discordLink)
            return null;

        var guildUser = GetDiscordIdAsUser(discordLink.DiscordUserId);

        if (guildUser == null)
            return null;

        for (var i = 0; i < _staffRoleIds.Length; i++)
        {
            if (guildUser.RoleIds.Contains(_staffRoleIds[i]))
                return i + 1;
        }

        return null;
    }

    public ActiveDiscordLink? GetLink(NetUserId userId) => _links.FirstOrNull(link => link.UserId == userId);

    public ActiveDiscordLink? GetLink(ulong discordId) => _links.FirstOrNull(link => link.DiscordUserId == discordId);

    public GuildUser? GetDiscordIdAsUser(ulong discordId)
    {
        if (_discordLink.Client == null || !_discordLink.Client.Cache.Guilds.TryGetValue(_discordLink.GuildId, out var guild))
            return null;

        return guild.Users.TryGetValue(discordId, out var user) ? user : null;
    }

    private async Task<GuildUser?> GetDiscordIdAsUserAsync(ulong discordId)
    {
        if (_discordLink.Client == null)
        {
            _sawmill.Info("invalid client");
            return null;
        }

        var user = await _discordLink.Client.Rest.GetGuildUserAsync(_discordLink.GuildId, discordId);
        _sawmill.Info($"{user.RoleIds.Count} roles found");

        return user;
    }

    private async void UpdatePlayerLink(ulong associatedDiscordId, ulong? newDiscordId)
    {
        await _db.UpdateDiscordLink(associatedDiscordId, newDiscordId);
    }

    private async void UpdatePlayerLink(NetUserId userId, ulong? newDiscordId)
    {
        await _db.UpdateDiscordLink(userId, newDiscordId);
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
