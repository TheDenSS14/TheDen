using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server._DEN.GameTicking.Rules;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking.Presets;
using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Configuration;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.Rules;

public sealed class SecretRuleSystem : GameRuleSystem<SecretRuleComponent>
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    protected override void Added(EntityUid uid, SecretRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, component, gameRule, args);
        var weights = _configurationManager.GetCVar(CCVars.SecretWeightPrototype);

        if (!_prototypeManager.TryIndex<WeightedRandomPrototype>(weights, out var weightedRandom))
        {
            Log.Error($"{ToPrettyString(uid)} failed to pick any preset. Removing rule.");
            Del(uid);
            return;
        }

        if (!_gameTicker.TryPickPreset(weightedRandom, out var preset))
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

    protected override void Ended(EntityUid uid, SecretRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        _gameTicker.EndGameRulesOf(component);
    }
}
