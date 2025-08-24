using System.Linq;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Player;


namespace Content.Server._DEN.Discord;


public sealed partial class DiscordUserLink
{
    private async Task UpdatePermissionsFromDiscord(ICommonSession session)
    {
        var actualRank = _adminManager.GetAdminRankId(session);
        var expectedRank = GetAdminRankOfRole(session.UserId);

        if (expectedRank == actualRank)
        {
            _sawmill.Info($"Expected rank is actual rank, ignoring. {expectedRank} is {actualRank}");
            return;
        }

        var admin = await _db.GetAdminDataForAsync(session.UserId);

        if (expectedRank == null && admin != null)
        {
            // await _db.RemoveAdminAsync(session.UserId);
            return;
        }

        var (bad, _) = await FetchAndCheckRank(expectedRank);

        if (bad)
        {
            _sawmill.Info("Rank doesn't exist.");
            return;
        }

        var realAdmin = admin ?? new Admin()
        {
            Suspended = false,
            Deadminned = false,
            Title = null,
            UserId = session.UserId
        };

        realAdmin.AdminRankId = expectedRank;

        if (admin != null && expectedRank != null)
        {
            await _db.UpdateAdminAsync(realAdmin);
        }
        else
        {
            await _db.AddAdminAsync(realAdmin);
        }

        _adminManager.ReloadAdmin(session);
    }

    private async Task<(bool bad, string?)> FetchAndCheckRank(int? rankId)
    {
        string? ret = null;

        if (rankId is not { } r)
            return (true, null);

        var rank = await _db.GetAdminRankAsync(r);
        if (rank == null)
            return (true, null);

        ret = rank.Name;

        return (false, ret);
    }
}
