using System.Threading;
using Content.Server.Voting;
using Content.Shared.CCVar;
using Robust.Shared.Player;
using Timer = Robust.Shared.Timing.Timer;


namespace Content.Server.RoundEnd;


public sealed partial class RoundEndSystem
{
    private void InitializeDen()
    {
        InitializeTimer();

        SubscribeLocalEvent<CanCallOrRecallEvent>(CheckIfCanCallOrRecall);
        SubscribeLocalEvent<ShuttleAutoCallAttemptedEvent>(OnShuttleAutoCallAttempted);
    }

    private TimeSpan WarnAt() => RoundHardEnd - RoundHardEndWarningTime;

    private void InitializeTimer()
    {
        Timer.Spawn(WarnAt(), SendWarningAnnouncement);
        Timer.Spawn(RoundHardEnd, UpdateRoundEnd);
    }

    private void CheckIfCanCallOrRecall(ref CanCallOrRecallEvent ev)
    {
        if (_gameTicker.RoundDuration() > RoundHardEnd && RespectRoundHardEnd)
            ev.Cancelled = true;
    }

    private void ShuttleRecallVoteFinished(VoteFinishedEventArgs args)
    {
        if (args.Winner == null)
            return;

        var votedYes = (bool) args.Winner;

        if (votedYes || !CanCallOrRecallIgnoringCooldown())
            return;

        RequestRoundEnd(null, false, "round-end-system-shuttle-auto-called-announcement");
        _autoCalledBefore = true;
    }

    /// <summary>
    /// Send recall vote.
    /// </summary>
    private void OnShuttleAutoCallAttempted(ref ShuttleAutoCallAttemptedEvent ev)
    {
        if (!CanCallOrRecallIgnoringCooldown())
            return;

        var alone = _playerManager.PlayerCount == 1;
        var options = new VoteOptions
        {
            Title = Loc.GetString("ui-vote-recall-title"),
            Duration = alone
                ? TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerAlone))
                : TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerRestart)) // just use restart vote timer
        };

        var localeYes = Loc.GetString("ui-vote-restart-yes");
        var localeNo = Loc.GetString("ui-vote-restart-no");

        options.InitiatorText = "Server";
        options.PlayVoteSound = false; // we expect to be doing this several times a shift.
        options.Options.Add((localeYes, true));
        options.Options.Add((localeNo, false));

        var recallVote = _voteManager.CreateVote(options);
        recallVote.OnFinished += (_, args) => ShuttleRecallVoteFinished(args);
    }

    public void UpdateRoundEnd() => RaiseLocalEvent(RoundEndSystemChangedEvent.Default);

    private void SendWarningAnnouncement()
    {
        if (!RespectRoundHardEnd)
            return;

        var warnAt = WarnAt();

        int time;
        string units;

        if (warnAt.TotalSeconds < 60)
        {
            time = warnAt.Seconds;
            units = "eta-units-seconds";
        }
        else
        {
            time = warnAt.Minutes;
            units = "eta-units-minutes";
        }

        _announcer.SendAnnouncement(
            _announcer.GetAnnouncementId("CommandReport"),
            Filter.Broadcast(),
            "round-end-system-shuttle-no-longer-recall-soon",
            "Station",
            Color.Gold,
            null,
            null,
            ("time", time),
            ("units", Loc.GetString(units))
        );
    }
}
