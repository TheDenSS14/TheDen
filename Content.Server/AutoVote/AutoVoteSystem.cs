// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Configuration;
using Content.Server.Voting.Managers;
using Content.Shared.GameTicking;
using Content.Shared.Voting;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Content.Server.GameTicking;
using Robust.Shared.Timing;


namespace Content.Server.AutoVote;

public sealed class AutoVoteSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = null!;
    [Dependency] private readonly IGameTiming _timing = null!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly IVoteManager _voteManager = null!;

    private bool _shouldVoteNextJoin;
    private TimeSpan? _voteStart;

    // CCVars
    private bool _autoVoteEnabled;
    private bool _mapVoteEnabled;
    private bool _presetVoteEnabled;
    private int _firstRoundDelaySeconds;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnReturnedToLobby);
        SubscribeLocalEvent<PlayerJoinedLobbyEvent>(OnPlayerJoinedLobby);

        _autoVoteEnabled = _cfg.GetCVar(CCVars.AutoVoteEnabled);
        _mapVoteEnabled = _cfg.GetCVar(CCVars.MapAutoVoteEnabled);
        _presetVoteEnabled = _cfg.GetCVar(CCVars.PresetAutoVoteEnabled);
        _firstRoundDelaySeconds = _cfg.GetCVar(CCVars.PresetAutoVoteFirstDelaySeconds);

        Subs.CVar(_cfg, CCVars.AutoVoteEnabled, OnAutoVoteStatusChanged, true);
        Subs.CVar(_cfg, CCVars.MapAutoVoteEnabled, OnMapVoteStatusChanged, true);
        Subs.CVar(_cfg, CCVars.PresetAutoVoteEnabled, OnPresetVoteStatusChanged, true);
        Subs.CVar(_cfg, CCVars.PresetAutoVoteFirstDelaySeconds, OnFirstRoundDelaySecondsChanged, true);
    }

    public override void Update(float deltaTime)
    {
        if (_voteStart == null)
            return;

        if (_voteStart > _timing.RealTime)
            return;

        CallAutovote();
        _voteStart = null;
    }

    public void OnReturnedToLobby(RoundRestartCleanupEvent ev) => CallAutovote();

    public void OnPlayerJoinedLobby(PlayerJoinedLobbyEvent ev)
    {
        if (!_shouldVoteNextJoin)
            return;

        _voteStart = _timing.RealTime + TimeSpan.FromSeconds(_firstRoundDelaySeconds);
        _shouldVoteNextJoin = false;
    }

    private void CallAutovote()
    {
        if (!_autoVoteEnabled)
            return;

        if (_playerManager.PlayerCount == 0)
        {
            _shouldVoteNextJoin = true;
            return;
        }

        if (_mapVoteEnabled)
            _voteManager.CreateStandardVote(null, StandardVoteType.Map);

        if (_presetVoteEnabled)
            _voteManager.CreateStandardVote(null, StandardVoteType.Preset);
    }

    private void OnAutoVoteStatusChanged(bool newValue) => _autoVoteEnabled = newValue;

    private void OnMapVoteStatusChanged(bool newValue) => _mapVoteEnabled = newValue;

    private void OnPresetVoteStatusChanged(bool newValue) => _presetVoteEnabled = newValue;

    private void OnFirstRoundDelaySecondsChanged(int newValue) => _firstRoundDelaySeconds = newValue;
}
