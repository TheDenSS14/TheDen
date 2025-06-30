// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.GameTicking;
using Content.Server.GameTicking.Presets;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Voting.Systems;


/// <summary>
/// This handles preventing duplicate votes
/// </summary>
public sealed class DuplicateVoteSystem : EntitySystem
{
    [Dependency] private readonly GameTicker _gameTicker = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public bool IsHighDanger;

    private bool _allowHighDanger = true;
    private int _lastHighDanger = -1;
    private int _roundsUntilHighDanger;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
        SubscribeLocalEvent<RoundStartedEvent>(OnRoundStarted);

        _roundsUntilHighDanger = _cfg.GetCVar(CCVars.LowDangerRoundsAfterHighDanger);

        Subs.CVar(_cfg,  CCVars.LowDangerRoundsAfterHighDanger, OnRoundsUntilHighDanger);
    }

    private void OnRoundsUntilHighDanger(int newValue) => _roundsUntilHighDanger = newValue;

    public bool IsHighDangerPickable() => _allowHighDanger;

    public void SetHighDangerPickable()
    {
        _allowHighDanger = true;
        _lastHighDanger = -1;
    }

    public void PresetVoteFinished(GamePresetPrototype preset)
    {
        if (!preset.HighDanger)
            return;

        _allowHighDanger = false;
        _lastHighDanger = _gameTicker.RoundId;
    }

    private void OnRoundStarted(RoundStartedEvent ev)
    {
        var preset = _gameTicker.CurrentPreset;

        if (preset != null)
            PresetVoteFinished(preset);

        if (ev.RoundId >= _lastHighDanger + _roundsUntilHighDanger)
            SetHighDangerPickable();
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        if (IsHighDanger)
        {
            SetHighDangerPickable();
            IsHighDanger = false;
        }
    }
}
