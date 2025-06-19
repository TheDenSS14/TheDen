// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server._DEN.GameTicking.Rules;
using Content.Server.GameTicking.Presets;
using Content.Server.PresetPicker;
using Content.Shared.CCVar;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;


namespace Content.Server.GameTicking;

/// <summary>
/// This handles loading a preset as if it was active when it is not, such as for Secret, Low Danger, or High Danger.
/// </summary>
public sealed partial class GameTicker
{
    [Dependency] private readonly IComponentFactory _componentFactory = default!;

    private string _ruleCompName = default!;

    public void InitializeFakePreset()
    {
        base.Initialize();

        _ruleCompName = _componentFactory.GetComponentName(typeof(GameRuleComponent));
    }

    public void StartGameRulesOf<T>(T component, GamePresetPrototype preset) where T : Component, IFakePreset
    {
        foreach (var rule in preset.Rules)
        {
            EntityUid ruleEnt;

            // if we're pre-round (i.e. will only be added)
            // then just add rules. if we're added in the middle of the round (or at any other point really)
            // then we want to start them as well
            if (RunLevel <= GameRunLevel.InRound)
                ruleEnt = AddGameRule(rule);
            else
                StartGameRule(rule, out ruleEnt);

            component.AdditionalGameRules.Add(ruleEnt);
        }
    }

    public void EndGameRulesOf<T>(T component) where T : Component, IFakePreset
    {
        foreach (var rule in component.AdditionalGameRules)
            EndGameRule(rule);
    }

    public bool TryPickPreset(ProtoId<PresetPickerPrototype> presetPickerId, [NotNullWhen(true)] out GamePresetPrototype? preset)
    {
        if (!_prototypeManager.TryIndex(presetPickerId, out var presetPicker))
        {
            preset = null;
            return false;
        }

        if (presetPicker.PossiblePresets != null && presetPicker.PossiblePresets.Any())
        {
            var presets = new List<GamePresetPrototype>();

            foreach (var targetPresetId in presetPicker.PossiblePresets)
            {
                var targetExists = _prototypeManager.TryIndex<GamePresetPrototype>(targetPresetId, out var targetPreset);

                if (!targetExists || !CanPick(targetPreset))
                    continue;

                presets.Add(targetPreset);
            }

            if (presets.Count == 0)
            {
                preset = null;
                return false;
            }

            preset = _robustRandom.Pick(presets);
            return true;
        }

        if (presetPicker.PossibleWeightedPresets != null && presetPicker.PossibleWeightedPresets.Any())
        {
            var exists = TryPickPreset(presetPicker.PossibleWeightedPresets, out preset);
            return exists;
        }

        preset = null;
        return false;
    }

    public bool TryPickPreset(ProtoId<WeightedRandomPrototype> weightedRandomId, [NotNullWhen(true)] out GamePresetPrototype? preset)
    {
        if (!_prototypeManager.TryIndex(weightedRandomId, out var weightedRandom))
        {
            preset = null;
            return false;
        }

        var exists = TryPickPreset(weightedRandom.Weights, out preset);
        return exists;
    }

    public bool TryPickPreset(Dictionary<string, float> chances, [NotNullWhen(true)] out GamePresetPrototype? preset)
    {
        if (!CanPickAny(chances.Keys))
        {
            preset = null;
            return false;
        }

        var weights = new Dictionary<string, float>();
        var players = ReadyPlayerCount();

        foreach (var (presetId, chance) in chances)
        {
            if (!_prototypeManager.TryIndex<GamePresetPrototype>(presetId, out var selected)
                || !CanPick(selected, players))
                continue;

            weights.Add(presetId, chance);
        }

        if (weights.Count == 0)
        {
            preset = null;
            return false;
        }

        var result = _robustRandom.Pick(weights);
        var exists = _prototypeManager.TryIndex(result, out preset);

        return exists;
    }

    public bool CanPickAny()
    {
        var secretPresetId = _configurationManager.GetCVar(CCVars.SecretWeightPrototype);
        return CanPickAny(secretPresetId);
    }

    /// <summary>
    /// Can any of the given presets be picked, taking into account the currently available player count?
    /// </summary>
    public bool CanPickAny(ProtoId<WeightedRandomPrototype> weightedPresets)
    {
        var weightedRandom = _prototypeManager.Index(weightedPresets);
        var ids = weightedRandom.Weights.Keys
            .Select(x => new ProtoId<GamePresetPrototype>(x));

        return CanPickAny(ids);
    }

    /// <summary>
    /// Can any of the given presets be picked, taking into account the currently available player count?
    /// </summary>
    public bool CanPickAny(IEnumerable<ProtoId<GamePresetPrototype>> protos)
    {
        var players = ReadyPlayerCount();
        foreach (var id in protos)
        {
            if (!_prototypeManager.TryIndex(id, out var selectedPreset))
                Log.Error($"Invalid preset {selectedPreset} in secret rule weights: {id}");

            if (CanPick(selectedPreset, players))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Can any of the given presets be picked, taking into account the currently available player count?
    /// </summary>
    public bool CanPickAny(IEnumerable<string> protos)
    {
        var players = ReadyPlayerCount();

        foreach (var id in protos)
        {
            if (!_prototypeManager.TryIndex<GamePresetPrototype>(id, out var selectedPreset))
                Log.Error($"Invalid preset {selectedPreset} in secret rule weights: {id}");

            if (CanPick(selectedPreset, players))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Can the given preset be picked, taking into account the currently available player count?
    /// </summary>
    public bool CanPick([NotNullWhen(true)] GamePresetPrototype? selected, int players)
    {
        if (selected == null)
            return false;

        foreach (var ruleId in selected.Rules)
        {
            if (!_prototypeManager.TryIndex(ruleId, out EntityPrototype? rule)
                || !rule.TryGetComponent(_ruleCompName, out GameRuleComponent? ruleComp))
            {
                Log.Error($"Encountered invalid rule {ruleId} in preset {selected.ID}");
                return false;
            }

            if (ruleComp.MinPlayers > players)
                return false;
        }

        return true;
    }

    /// <inherited/>
    public bool CanPick([NotNullWhen(true)] GamePresetPrototype? selected)
    {
        var players = ReadyPlayerCount();

        if (selected == null)
            return false;

        foreach (var ruleId in selected.Rules)
        {
            if (!_prototypeManager.TryIndex(ruleId, out EntityPrototype? rule)
                || !rule.TryGetComponent(_ruleCompName, out GameRuleComponent? ruleComp))
            {
                Log.Error($"Encountered invalid rule {ruleId} in preset {selected.ID}");
                return false;
            }

            if (ruleComp.MinPlayers > players)
                return false;
        }

        return true;
    }
}
