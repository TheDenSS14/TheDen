// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2021 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2021 moonheart08 <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 2022 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Putnam3145 <putnam3145@gmail.com>
// SPDX-FileCopyrightText: 2022 Radosvik <65792927+Radosvik@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 corentt <36075110+corentt@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Chief-Engineer <119664036+Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 LankLTE <135308300+LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 alexkar598 <25136265+alexkar598@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Repo <47093363+Titian3@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server._DEN.Voting.Systems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Presets;
using Content.Server.Maps;
using Content.Server.PresetPicker;
using Content.Server.RoundEnd;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Ghost;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Voting;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Voting.Managers;


public sealed partial class VoteManager
{
    private static readonly ProtoId<GamePresetPrototype> DefaultGamePreset = "Greenshift";

    private static readonly Dictionary<StandardVoteType, CVarDef<bool>> _voteTypesToEnableCVars = new()
    {
        {StandardVoteType.Restart, CCVars.VoteRestartEnabled},
        {StandardVoteType.Preset, CCVars.VotePresetEnabled},
        {StandardVoteType.Map, CCVars.VoteMapEnabled},
    };

    public void CreateStandardVote(ICommonSession? initiator, StandardVoteType voteType)
    {
        if (initiator != null)
            _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"{initiator} initiated a {voteType.ToString()} vote");
        else
            _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Initiated a {voteType.ToString()} vote");

        switch (voteType)
        {
            case StandardVoteType.Restart:
                CreateRestartVote(initiator);
                break;
            case StandardVoteType.Preset:
                CreatePresetVote(initiator);
                break;
            case StandardVoteType.Map:
                CreateMapVote(initiator);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(voteType), voteType, null);
        }
        var ticker = _entityManager.EntitySysManager.GetEntitySystem<GameTicker>();
        ticker.UpdateInfoText();
        TimeoutStandardVote(voteType);
    }

    private void CreateRestartVote(ICommonSession? initiator)
    {

        var playerVoteMaximum = _cfg.GetCVar(CCVars.VoteRestartMaxPlayers);
        var totalPlayers = _playerManager.Sessions.Count(session => session.Status != SessionStatus.Disconnected);

        var ghostVotePercentageRequirement = _cfg.GetCVar(CCVars.VoteRestartGhostPercentage);
        var ghostCount = 0;

        foreach (var player in _playerManager.Sessions)
        {
            _playerManager.UpdateState(player);
            if (player.Status != SessionStatus.Disconnected && _entityManager.HasComponent<GhostComponent>(player.AttachedEntity))
            {
                ghostCount++;
            }
        }

        var ghostPercentage = 0.0;
        if (totalPlayers > 0)
        {
            ghostPercentage = ((double)ghostCount / totalPlayers) * 100;
        }

        var roundedGhostPercentage = (int)Math.Round(ghostPercentage);

        if (totalPlayers <= playerVoteMaximum || roundedGhostPercentage >= ghostVotePercentageRequirement)
        {
            StartVote(initiator);
        }
        else
        {
            NotifyNotEnoughGhostPlayers(ghostVotePercentageRequirement, roundedGhostPercentage);
        }
    }

    private void StartVote(ICommonSession? initiator)
    {
        var alone = _playerManager.PlayerCount == 1 && initiator != null;
        var options = new VoteOptions
        {
            Title = Loc.GetString("ui-vote-restart-title"),
            Options =
            {
                (Loc.GetString("ui-vote-restart-yes"), "yes"),
                (Loc.GetString("ui-vote-restart-no"), "no"),
                (Loc.GetString("ui-vote-restart-abstain"), "abstain")
            },
            Duration = alone
                ? TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerAlone))
                : TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerRestart)),
            InitiatorTimeout = TimeSpan.FromMinutes(5)
        };

        if (alone)
            options.InitiatorTimeout = TimeSpan.FromSeconds(10);

        WirePresetVoteInitiator(options, initiator);

        var vote = CreateVote(options);

        vote.OnFinished += (_, _) =>
        {
            var votesYes = vote.VotesPerOption["yes"];
            var votesNo = vote.VotesPerOption["no"];
            var total = votesYes + votesNo;

            var ratioRequired = _cfg.GetCVar(CCVars.VoteRestartRequiredRatio);
            if (total > 0 && votesYes / (float) total >= ratioRequired)
            {
                // Check if an admin is online, and ignore the passed vote if the cvar is enabled
                if (_cfg.GetCVar(CCVars.VoteRestartNotAllowedWhenAdminOnline) && _adminMgr.ActiveAdmins.Count() != 0)
                {
                    _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Restart vote attempted to pass, but an admin was online. {votesYes}/{votesNo}");
                }
                else // If the cvar is disabled or there's no admins on, proceed as normal
                {
                    _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Restart vote succeeded: {votesYes}/{votesNo}");
                    _chatManager.DispatchServerAnnouncement(Loc.GetString("ui-vote-restart-succeeded"));
                    var roundEnd = _entityManager.EntitySysManager.GetEntitySystem<RoundEndSystem>();
                    roundEnd.EndRound();
                }
            }
            else
            {
                _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Restart vote failed: {votesYes}/{votesNo}");
                _chatManager.DispatchServerAnnouncement(
                    Loc.GetString("ui-vote-restart-failed", ("ratio", ratioRequired)));
            }
        };

        if (initiator != null)
        {
            // Cast yes vote if created the vote yourself.
            vote.CastVote(initiator, 0);
        }

        foreach (var player in _playerManager.Sessions)
        {
            if (player != initiator)
            {
                // Everybody else defaults to an abstain vote to say they don't mind.
                vote.CastVote(player, 2);
            }
        }
    }

    private void NotifyNotEnoughGhostPlayers(int ghostPercentageRequirement, int roundedGhostPercentage)
    {
        // Logic to notify that there are not enough ghost players to start a vote
        _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Restart vote failed: Current Ghost player percentage:{roundedGhostPercentage.ToString()}% does not meet {ghostPercentageRequirement.ToString()}%");
        _chatManager.DispatchServerAnnouncement(
            Loc.GetString("ui-vote-restart-fail-not-enough-ghost-players", ("ghostPlayerRequirement", ghostPercentageRequirement)));
    }

    private void CreatePresetVote(ICommonSession? initiator)
    {
        var duplicateVote = _entityManager.EntitySysManager.GetEntitySystem<DuplicateVoteSystem>();
        var ticker = _entityManager.EntitySysManager.GetEntitySystem<GameTicker>();
        var presets = GetGamePresets();

        var alone = _playerManager.PlayerCount == 1 && initiator != null;
        var options = new VoteOptions
        {
            Title = Loc.GetString("ui-vote-gamemode-title"),
            Duration = alone
                ? TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerAlone))
                : TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerPreset))
        };

        if (alone)
            options.InitiatorTimeout = TimeSpan.FromSeconds(10);

        foreach (var preset in presets)
        {
            if (preset.HighDanger && !duplicateVote.IsHighDangerPickable() || !ticker.CanPick(preset))
                continue;

            var properModeTitle = Loc.GetString(preset.ModeTitle);
            options.Options.Add((properModeTitle, preset.ID));
        }

        WirePresetVoteInitiator(options, initiator);

        var vote = CreateVote(options);

        vote.OnFinished += (_, args) =>
        {
            string picked;
            GamePresetPrototype presetPrototype;

            if (args.Winner == null)
            {
                picked = (string) _random.Pick(args.Winners);
                presetPrototype = _prototypeManager.Index<GamePresetPrototype>(picked);

                _chatManager.DispatchServerAnnouncement(
                    Loc.GetString("ui-vote-gamemode-tie", ("picked", Loc.GetString(presetPrototype.ModeTitle))));
            }
            else
            {
                picked = (string) args.Winner;
                presetPrototype = _prototypeManager.Index<GamePresetPrototype>(picked);

                _chatManager.DispatchServerAnnouncement(
                    Loc.GetString("ui-vote-gamemode-win", ("winner", Loc.GetString(presetPrototype.ModeTitle))));
            }

            _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Preset vote finished: {picked}");
            ticker.SetGamePreset(picked);
        };
    }

    private void CreateMapVote(ICommonSession? initiator)
    {
        var maps = _gameMapManager.CurrentlyEligibleMaps().ToDictionary(map => map, map => map.MapName);

        var alone = _playerManager.PlayerCount == 1 && initiator != null;
        var options = new VoteOptions
        {
            Title = Loc.GetString("ui-vote-map-title"),
            Duration = alone
                ? TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerAlone))
                : TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteTimerMap))
        };

        if (alone)
            options.InitiatorTimeout = TimeSpan.FromSeconds(10);

        foreach (var (k, v) in maps)
        {
            options.Options.Add((v, k));
        }

        WirePresetVoteInitiator(options, initiator);

        var vote = CreateVote(options);

        vote.OnFinished += (_, args) =>
        {
            GameMapPrototype picked;
            if (args.Winner == null)
            {
                picked = (GameMapPrototype) _random.Pick(args.Winners);
                _chatManager.DispatchServerAnnouncement(
                    Loc.GetString("ui-vote-map-tie", ("picked", maps[picked])));
            }
            else
            {
                picked = (GameMapPrototype) args.Winner;
                _chatManager.DispatchServerAnnouncement(
                    Loc.GetString("ui-vote-map-win", ("winner", maps[picked])));
            }

            _adminLogger.Add(LogType.Vote, LogImpact.Medium, $"Map vote finished: {picked.MapName}");
            var ticker = _entityManager.EntitySysManager.GetEntitySystem<GameTicker>();
            if (ticker.CanUpdateMap())
            {
                if (_gameMapManager.TrySelectMapIfEligible(picked.ID))
                {
                    ticker.UpdateInfoText();
                }
            }
            else
            {
                if (ticker.RoundPreloadTime <= TimeSpan.Zero)
                {
                    _chatManager.DispatchServerAnnouncement(Loc.GetString("ui-vote-map-notlobby"));
                }
                else
                {
                    var timeString = $"{ticker.RoundPreloadTime.Minutes:0}:{ticker.RoundPreloadTime.Seconds:00}";
                    _chatManager.DispatchServerAnnouncement(Loc.GetString("ui-vote-map-notlobby-time", ("time", timeString)));
                }
            }
        };
    }

    private void TimeoutStandardVote(StandardVoteType type)
    {
        var timeout = TimeSpan.FromSeconds(_cfg.GetCVar(CCVars.VoteSameTypeTimeout));
        _standardVoteTimeout[type] = _timing.RealTime + timeout;
        DirtyCanCallVoteAll();
    }

    private List<GamePresetPrototype> GetGamePresets()
    {
        var presets = new List<GamePresetPrototype>();

        foreach (var preset in _prototypeManager.EnumeratePrototypes<GamePresetPrototype>())
        {
            if (!preset.ShowInVote)
                continue;

            if (_playerManager.PlayerCount < (preset.MinPlayers ?? int.MinValue))
                continue;

            if (_playerManager.PlayerCount > (preset.MaxPlayers ?? int.MaxValue))
                continue;

            presets.Add(preset);
        }

        return presets;
    }
}
