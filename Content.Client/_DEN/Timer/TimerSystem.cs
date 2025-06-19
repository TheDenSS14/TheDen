// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Client.Chat;
using Content.Shared.GameTicking;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio.Sources;


namespace Content.Client._DEN.Timer;


/// <summary>
/// This handles timers.
/// </summary>
public sealed class TimerSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly SharedGameTicker _gameTicker = default!;
    [Dependency] private readonly IResourceCache _res = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    private const string Sound = "/Audio/Effects/voteding.ogg";
    private readonly TimeSpan _timerCheckInterval = TimeSpan.FromSeconds(1);

    private ISawmill _sawmill = default!;
    private IAudioSource? _audioSource;
    private TimeSpan _lastTimer = TimeSpan.Zero;
    private List<DenTimerInfo> _timers = new();

    /// <inheritdoc/>
    public override void Initialize()
    {
        var audioResource = _res.GetResource<AudioResource>(Sound);

        _audioSource = _audioManager.CreateAudioSource(audioResource);
        _sawmill = _logManager.GetSawmill("den.timer");

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        _lastTimer = TimeSpan.Zero;
        _timers.Clear();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_timers.Count == 0 || _lastTimer + _timerCheckInterval > _gameTicker.RoundDuration())
            return;

        var listTimers = _timers.ToList();

        foreach (var timer in listTimers)
        {
            if (timer.TryRun())
                _timers.Remove(timer);
        }

        _lastTimer = _gameTicker.RoundDuration();
    }

    public void AddTimer(TimeSpan runAt, Action action)
    {
        var timerInfo = new DenTimerInfo(_gameTicker, runAt, action);
        _timers.Add(timerInfo);
    }

    public void AddSimpleTimer(TimeSpan runAt) =>
        AddTimer(runAt, RespondWithAudio);

    private void RespondWithAudio()
    {
        _audioSource?.Restart();
        _sawmill.Info("Timer ended!");
    }

    private record struct DenTimerInfo
    {
        private readonly TimeSpan _runAt;
        private readonly Action _action;
        private readonly SharedGameTicker _gameTicker;

        public DenTimerInfo(SharedGameTicker gameTicker, TimeSpan runAt, Action action)
        {
            _gameTicker = gameTicker;
            _runAt = runAt;
            _action = action;
        }

        public bool TryRun()
        {
            if (_gameTicker.RoundDuration() < _runAt)
                return false;

            _action();
            return true;
        }
    }
}
