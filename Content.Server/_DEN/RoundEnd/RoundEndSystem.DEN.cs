using System.Threading;
using Robust.Shared.Player;
using Timer = Robust.Shared.Timing.Timer;


namespace Content.Server.RoundEnd;


public sealed partial class RoundEndSystem
{
    private CancellationTokenSource? _roundHardEndWarningToken;

    private void InitializeDen()
    {
        InitializeTimer();

        SubscribeLocalEvent<CanCallOrRecallEvent>(CheckIfCanCallOrRecall);
    }

    public void RestartHardEndWarning()
    {
        _roundHardEndWarningToken?.Cancel();

        InitializeTimer();
    }

    public void CancelHardEndWarning() => _roundHardEndWarningToken?.Cancel();

    private TimeSpan WarnAt() => RoundHardEnd - RoundHardEndWarningTime;

    private void InitializeTimer()
    {
        _roundHardEndWarningToken = new();

        Timer.Spawn(WarnAt(), SendWarningAnnouncement, _roundHardEndWarningToken.Token);
    }

    private void CheckIfCanCallOrRecall(ref CanCallOrRecallEvent ev)
    {
        if (_gameTicker.RoundDuration() > RoundHardEnd && RespectRoundHardEnd)
            ev.Cancelled = true;
    }

    private void SendWarningAnnouncement()
    {
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
