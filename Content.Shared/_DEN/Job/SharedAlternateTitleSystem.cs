using System.Linq;
using Robust.Shared.Network;
using Robust.Shared.Utility;


namespace Content.Shared._DEN.Job;

/// <summary>
/// This handles storing and loading player-saved alternate titles.
/// </summary>
public abstract partial class SharedAlternateTitleSystem : EntitySystem
{
    private HashSet<PlayerAlternateTitle> _alternateTitles = [];

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _alternateTitles.Clear();
    }

    public virtual void SetAlternateTitle(NetUserId userId, string jobId, string? newTitle)
    {
        _alternateTitles.RemoveWhere(title => title.JobId == jobId && title.UserId == userId);

        if (newTitle == null)
            return;

        _alternateTitles.Add(new(userId, jobId, newTitle));
    }

    public HashSet<PlayerAlternateTitle> GetAlternateTitles() =>
        _alternateTitles;

    public virtual PlayerAlternateTitle? GetAlternateTitle(NetUserId userId, string jobId) =>
        _alternateTitles
            .Where(title => title.JobId == jobId && title.UserId == userId)
            .FirstOrNull();

    public virtual List<PlayerAlternateTitle> GetAlternateTitles(NetUserId userId) =>
        _alternateTitles
            .Where(title => title.UserId == userId)
            .ToList();

    public virtual void ClearAlternateTitles(NetUserId userId) =>
        _alternateTitles.RemoveWhere(title => title.UserId == userId);
}

public record struct PlayerAlternateTitle(NetUserId UserId, string JobId, string AlternateTitle)
{
    public NetUserId UserId { get; init; } = UserId;
    public string JobId { get; init; } = JobId;
    public string AlternateTitle { get; init; } = AlternateTitle;
}
