using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration.Managers;
using Robust.Shared.Player;

namespace Content.Server.Players.PlayTimeTracking;

public sealed partial class PlayTimeTrackingManager
{
    [Dependency] private readonly IBanManager _banManager = default!;

    public bool TryGetRoleBans(ICommonSession id, [NotNullWhen(true)] out HashSet<string>? bans)
    {
        bans = _banManager.GetRoleBans(id.UserId);
        return bans != null;
    }
}
