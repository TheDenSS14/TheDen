using Content.Server.GameTicking;
using Content.Shared.Dataset;
using Content.Shared.Random.Helpers;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Announcements.Systems;

public sealed partial class AnnouncerSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly GameTicker _ticker = default!;

    public void AnnounceChangedAnnouncer(string? changeReason = null)
    {
        // If we're not even in the round yet, then the announcer doesn't "exist" yet to the players.
        // Thus, it'd be silly to create a message stating the announcer has changed!
        if (_ticker.RunLevel == GameRunLevel.PreRoundLobby)
            return;

        var announcementId = GetAnnouncementId("AnnouncerChanged");
        var filter = Filter.Broadcast();
        var announcementLocale = "announcer-changed-announcement";
        (string, object)[] localeArgs = [("reason", changeReason ?? GetReason())];

        SendAnnouncement(announcementId,
            filter,
            announcementLocale,
            sender: null,
            colorOverride: null,
            station: null,
            announcerOverride: null,
            localeArgs: localeArgs);
    }

    private string GetReason()
    {
        var datasetId = "AnnouncerChangedReasons";

        if (!_proto.TryIndex<LocalizedDatasetPrototype>(datasetId, out var reasonDataset))
            return Loc.GetString("announcer-changed-reason-1");

        return _random.Pick(reasonDataset);
    }
}
