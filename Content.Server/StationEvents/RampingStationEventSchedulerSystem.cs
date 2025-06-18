// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Server.StationEvents.Events;
using Content.Shared.CCVar;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Configuration;
using Robust.Shared.Random;

namespace Content.Server.StationEvents;

public sealed class RampingStationEventSchedulerSystem : GameRuleSystem<RampingStationEventSchedulerComponent>
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EventManagerSystem _event = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    public float GetChaosModifier(EntityUid uid, RampingStationEventSchedulerComponent component)
    {
        var roundTime = (float) _gameTicker.RoundDuration().TotalSeconds;
        if (roundTime > component.EndTime)
            return component.MaxChaos;

        return component.MaxChaos / component.EndTime * roundTime + component.StartingChaos;
    }

    protected override void Started(EntityUid uid, RampingStationEventSchedulerComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        var avgChaos = _cfg.GetCVar(CCVars.EventsRampingAverageChaos) * component.ChaosModifier;
        var avgTime = _cfg.GetCVar(CCVars.EventsRampingAverageEndTime) * component.ShiftLengthModifier;

        // Worlds shittiest probability distribution
        // Got a complaint? Send them to
        component.MaxChaos = avgChaos * _random.NextFloat(0.75f, 1.25f);
        // This is in minutes, so *60 for seconds (for the chaos calc)
        component.EndTime = avgTime * _random.NextFloat(0.75f, 1.25f) * 60f;
        component.StartingChaos = component.MaxChaos * component.StartingChaosRatio;

        PickNextEventTime(uid, component);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_event.EventsEnabled)
            return;

        var query = EntityQueryEnumerator<RampingStationEventSchedulerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var scheduler, out var gameRule))
        {
            if (!GameTicker.IsGameRuleActive(uid, gameRule))
                return;

            if (scheduler.TimeUntilNextEvent > 0f)
            {
                scheduler.TimeUntilNextEvent -= frameTime;
                return;
            }

            PickNextEventTime(uid, scheduler);
            _event.RunRandomEvent();
        }
    }

    private void PickNextEventTime(EntityUid uid, RampingStationEventSchedulerComponent component)
    {
        component.TimeUntilNextEvent = _random.NextFloat(
            _cfg.GetCVar(CCVars.GameEventsRampingMinimumTime),
            _cfg.GetCVar(CCVars.GameEventsRampingMaximumTime));

        component.TimeUntilNextEvent *= component.EventDelayModifier / GetChaosModifier(uid, component);
    }
}
