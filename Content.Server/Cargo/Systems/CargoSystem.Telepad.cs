// SPDX-FileCopyrightText: 2022 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Fishfish458 <47410468+Fishfish458@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Marat Gadzhiev <15rinkashikachi15@gmail.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 fishfish458 <fishfish458>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Checkraze <71046427+Cheackraze@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 2023 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2023 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
// SPDX-FileCopyrightText: 2023 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.Construction;
using Content.Server.Paper;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Station.Components;
using Content.Shared.Cargo;
using Content.Shared.Cargo.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.Power;
using Robust.Shared.Audio;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Cargo.Systems;

public sealed partial class CargoSystem
{
    private void InitializeTelepad()
    {
        SubscribeLocalEvent<CargoTelepadComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<CargoTelepadComponent, RefreshPartsEvent>(OnRefreshParts);
        SubscribeLocalEvent<CargoTelepadComponent, UpgradeExamineEvent>(OnUpgradeExamine);
        SubscribeLocalEvent<CargoTelepadComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<CargoTelepadComponent, PowerChangedEvent>(OnTelepadPowerChange);
        // Shouldn't need re-anchored event
        SubscribeLocalEvent<CargoTelepadComponent, AnchorStateChangedEvent>(OnTelepadAnchorChange);
        SubscribeLocalEvent<FulfillCargoOrderEvent>(OnTelepadFulfillCargoOrder);
    }

    private void OnTelepadFulfillCargoOrder(ref FulfillCargoOrderEvent args)
    {
        var query = EntityQueryEnumerator<CargoTelepadComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var tele, out var xform))
        {
            if (tele.CurrentState != CargoTelepadState.Idle)
                continue;

            if (!this.IsPowered(uid, EntityManager))
                continue;

            if (_station.GetOwningStation(uid, xform) != args.Station)
                continue;

            // todo cannot be fucking asked to figure out device linking rn but this shouldn't just default to the first port.
            if (!TryComp<DeviceLinkSinkComponent>(uid, out var sinkComponent) ||
                sinkComponent.LinkedSources.FirstOrNull() is not { } console ||
                console != args.OrderConsole.Owner)
                continue;

            for (var i = 0; i < args.Order.OrderQuantity; i++)
            {
                tele.CurrentOrders.Add(args.Order);
            }
            tele.Accumulator = tele.Delay;
            args.Handled = true;
            args.FulfillmentEntity = uid;
            return;
        }
    }

    private void UpdateTelepad(float frameTime)
    {
        var query = EntityQueryEnumerator<CargoTelepadComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // Don't EntityQuery for it as it's not required.
            TryComp<AppearanceComponent>(uid, out var appearance);

            if (comp.CurrentState == CargoTelepadState.Unpowered)
            {
                comp.CurrentState = CargoTelepadState.Idle;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Idle, appearance);
                comp.Accumulator = comp.Delay;
                continue;
            }

            comp.Accumulator -= frameTime;

            // Uhh listen teleporting takes time and I just want the 1 float.
            if (comp.Accumulator > 0f)
            {
                comp.CurrentState = CargoTelepadState.Idle;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Idle, appearance);
                continue;
            }

            if (comp.CurrentOrders.Count == 0)
            {
                comp.Accumulator += comp.Delay;
                continue;
            }

            var xform = Transform(uid);
            var currentOrder = comp.CurrentOrders.First();
            if (FulfillOrder(currentOrder, xform.Coordinates, comp.PrinterOutput))
            {
                _audio.PlayPvs(_audio.GetSound(comp.TeleportSound), uid, AudioParams.Default.WithVolume(-8f));

                if (_station.GetOwningStation(uid) is { } station)
                    UpdateOrders(station);

                comp.CurrentOrders.Remove(currentOrder);
                comp.CurrentState = CargoTelepadState.Teleporting;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Teleporting, appearance);
            }

            comp.Accumulator += comp.Delay;
        }
    }

    private void OnInit(EntityUid uid, CargoTelepadComponent telepad, ComponentInit args)
    {
        _linker.EnsureSinkPorts(uid, telepad.ReceiverPort);
    }

    private void OnRefreshParts(EntityUid uid, CargoTelepadComponent component, RefreshPartsEvent args)
    {
        var rating = args.PartRatings[component.MachinePartTeleportDelay] - 1;
        component.Delay = component.BaseDelay * MathF.Pow(component.PartRatingTeleportDelay, rating);
    }

    private void OnUpgradeExamine(EntityUid uid, CargoTelepadComponent component, UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade("cargo-telepad-delay-upgrade", component.Delay / component.BaseDelay);
    }

    private void OnShutdown(Entity<CargoTelepadComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.CurrentOrders.Count == 0)
            return;

        if (_station.GetStations().Count == 0)
            return;

        if (_station.GetOwningStation(ent) is not { } station)
        {
            station = _random.Pick(_station.GetStations().Where(HasComp<StationCargoOrderDatabaseComponent>).ToList());
        }

        if (!TryComp<StationCargoOrderDatabaseComponent>(station, out var db) ||
            !TryComp<StationDataComponent>(station, out var data))
            return;

        foreach (var order in ent.Comp.CurrentOrders)
        {
            TryFulfillOrder((station, data), order, db);
        }
    }

    private void SetEnabled(EntityUid uid, CargoTelepadComponent component, ApcPowerReceiverComponent? receiver = null,
        TransformComponent? xform = null)
    {
        // False due to AllCompsOneEntity test where they may not have the powerreceiver.
        if (!Resolve(uid, ref receiver, ref xform, false))
            return;

        var disabled = !receiver.Powered || !xform.Anchored;

        // Setting idle state should be handled by Update();
        if (disabled)
            return;

        TryComp<AppearanceComponent>(uid, out var appearance);
        component.CurrentState = CargoTelepadState.Unpowered;
        _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Unpowered, appearance);
    }

    private void OnTelepadPowerChange(EntityUid uid, CargoTelepadComponent component, ref PowerChangedEvent args)
    {
        SetEnabled(uid, component);
    }

    private void OnTelepadAnchorChange(EntityUid uid, CargoTelepadComponent component, ref AnchorStateChangedEvent args)
    {
        SetEnabled(uid, component);
    }
}
