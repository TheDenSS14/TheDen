// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading;
using Content.Server.GameTicking.Events;
using Content.Server.Voting;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Robust.Shared.Player;
using Timer = Robust.Shared.Timing.Timer;


namespace Content.Server.RoundEnd;


public sealed partial class RoundEndSystem
{
    private CancellationTokenSource? _timerCancellation;

    private bool _hasHardEndWarningRun;

    private void InitializeDen()
    {
        SubscribeLocalEvent<CanCallOrRecallEvent>(CheckIfCanCallOrRecall);
    }

    private TimeSpan WarnAt() => RoundHardEnd - RoundHardEndWarningTime;

    private void UpdateForWarning()
    {
        if (_hasHardEndWarningRun || _gameTicker.RoundDuration() < WarnAt())
            return;

        _hasHardEndWarningRun = true;
        SendWarningAnnouncement();
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
        var logText = votedYes ? "staying" : "leaving";

        _adminLogger.Add(LogType.Vote, LogImpact.Low, $"Round extension vote ended in favor of {logText}.");

        if (votedYes)
            return;

        RequestRoundEnd(null, false, "round-end-system-shuttle-auto-called-announcement");
        _autoCalledBefore = true;
    }

    /// <summary>
    /// Send recall vote.
    /// </summary>
    private void CreateAutoCallVote()
    {
        if (RoundHardEnd - _gameTicker.RoundDuration() < TimeSpan.FromMinutes(30))
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

        _adminLogger.Add(LogType.Vote, LogImpact.Low, $"Server vote started for round extension.");

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

        if (RoundHardEndWarningTime.TotalSeconds < 60)
        {
            time = RoundHardEndWarningTime.Seconds;
            units = "eta-units-seconds";
        }
        else
        {
            time = RoundHardEndWarningTime.Minutes;
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
