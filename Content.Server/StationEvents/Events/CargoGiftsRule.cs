// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.Cargo.Systems;
using Content.Server.GameTicking;
using Content.Server.Station.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Prototypes;
using Content.Server.Announcements.Systems;
using Robust.Shared.Player;

namespace Content.Server.StationEvents.Events;

public sealed class CargoGiftsRule : StationEventSystem<CargoGiftsRuleComponent>
{
    [Dependency] private readonly CargoSystem _cargoSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly GameTicker _ticker = default!;
    [Dependency] private readonly AnnouncerSystem _announcer = default!;

    protected override void Added(EntityUid uid, CargoGiftsRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, component, gameRule, args);

        _announcer.SendAnnouncement(
            _announcer.GetAnnouncementId(args.RuleId),
            Filter.Broadcast(),
            component.Announce,
            null,
            Color.FromHex("#18abf5"),
            null, null,
            ("sender", Loc.GetString(component.Sender)),
                ("description", Loc.GetString(component.Description)),
                ("dest", Loc.GetString(component.Dest))
        );
    }

    /// <summary>
    /// Called on an active gamerule entity in the Update function
    /// </summary>
    protected override void ActiveTick(EntityUid uid, CargoGiftsRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        if (component.Gifts.Count == 0)
            return;

        if (component.TimeUntilNextGifts > 0)
        {
            component.TimeUntilNextGifts -= frameTime;
            return;
        }

        component.TimeUntilNextGifts += 30f;

        if (!TryGetRandomStation(out var station, HasComp<StationCargoOrderDatabaseComponent>) ||
                !TryComp<StationDataComponent>(station, out var stationData))
            return;

        if (!TryComp<StationCargoOrderDatabaseComponent>(station, out var cargoDb))
        {
            return;
        }

        // Add some presents
        var outstanding = CargoSystem.GetOutstandingOrderCount(cargoDb);
        while (outstanding < cargoDb.Capacity - component.OrderSpaceToLeave && component.Gifts.Count > 0)
        {
            // I wish there was a nice way to pop this
            var (productId, qty) = component.Gifts.First();
            component.Gifts.Remove(productId);

            var product = _prototypeManager.Index(productId);

            if (!_cargoSystem.AddAndApproveOrder(
                    station!.Value,
                    product.Product,
                    product.Name,
                    product.Cost,
                    qty,
                    Loc.GetString(component.Sender),
                    Loc.GetString(component.Description),
                    Loc.GetString(component.Dest),
                    cargoDb,
                    (station.Value, stationData)
            ))
            {
                break;
            }
        }

        if (component.Gifts.Count == 0)
        {
            // We're done here!
            _ticker.EndGameRule(uid, gameRule);
        }
    }

}
