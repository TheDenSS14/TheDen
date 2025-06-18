// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Presets;
using Content.Server.GameTicking.Rules;
using Content.Server.PresetPicker;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;


namespace Content.Server._DEN.GameTicking.Rules;


/// <summary>
/// This handles the Low Danger rule.
/// </summary>
public sealed class LowDangerRuleSystem : GameRuleSystem<LowDangerRuleComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    private readonly ProtoId<PresetPickerPrototype> _lowDangerProtoId = "LowDanger";

    protected override void Added(
        EntityUid uid,
        LowDangerRuleComponent component,
        GameRuleComponent gameRule,
        GameRuleAddedEvent args
    )
    {
        base.Added(uid, component, gameRule, args);

        // Not checking because I want it to error if it's gone.
        var presetPicker = _prototypeManager.Index(_lowDangerProtoId);

        if (!_gameTicker.TryPickPreset(presetPicker, out var preset))
        {
            Log.Error($"{ToPrettyString(uid)} failed to pick any preset. Removing rule.");
            Del(uid);
            return;
        }

        Log.Info($"Selected {preset.ID} as the secret preset.");
        _adminLogger.Add(LogType.EventStarted, $"Selected {preset.ID} as the secret preset.");
        _chatManager.SendAdminAnnouncement(Loc.GetString("rule-secret-selected-preset", ("preset", preset.ID)));
        _gameTicker.StartGameRulesOf(component, preset);
    }

    protected override void Ended(
        EntityUid uid,
        LowDangerRuleComponent component,
        GameRuleComponent gameRule,
        GameRuleEndedEvent args
    )
    {
        base.Ended(uid, component, gameRule, args);

        foreach (var rule in component.AdditionalGameRules)
            GameTicker.EndGameRule(rule);
    }
}
