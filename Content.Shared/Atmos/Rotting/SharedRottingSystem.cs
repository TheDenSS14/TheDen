// SPDX-FileCopyrightText: 2024 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 XavierSomething <tylernguyen203@gmail.com>
// SPDX-FileCopyrightText: 2025 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DEN.Unrotting;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Rejuvenate;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Content.Shared.Cuffs.Components;
using JetBrains.Annotations;
using Content.Shared.Nutrition.Components;

namespace Content.Shared.Atmos.Rotting;

public abstract class SharedRottingSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public const int MaxStages = 3;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PerishableComponent, MapInitEvent>(OnPerishableMapInit);
        SubscribeLocalEvent<PerishableComponent, MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<PerishableComponent, ExaminedEvent>(OnPerishableExamined);

        SubscribeLocalEvent<RottingComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<RottingComponent, MobStateChangedEvent>(OnRottingMobStateChanged);
        SubscribeLocalEvent<RottingComponent, RejuvenateEvent>(OnRejuvenate);
        SubscribeLocalEvent<RottingComponent, ExaminedEvent>(OnExamined);
    }

    private void OnPerishableMapInit(EntityUid uid, PerishableComponent component, MapInitEvent args)
    {
        component.RotNextUpdate = _timing.CurTime + component.PerishUpdateRate;
    }

    private void OnMobStateChanged(EntityUid uid, PerishableComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead && args.OldMobState != MobState.Dead)
            return;

        if (HasComp<RottingComponent>(uid))
            return;

        component.RotAccumulator = TimeSpan.Zero;
        component.RotNextUpdate = _timing.CurTime + component.PerishUpdateRate;
    }

    private void OnPerishableExamined(Entity<PerishableComponent> perishable, ref ExaminedEvent args)
    {
        var examineText = GetPerishableExamineText(perishable);
        args.PushMarkup(examineText);
    }

    private void OnShutdown(EntityUid uid, RottingComponent component, ComponentShutdown args)
    {
        if (TryComp<PerishableComponent>(uid, out var perishable))
        {
            perishable.RotNextUpdate = TimeSpan.Zero;
        }
    }

    private void OnRottingMobStateChanged(EntityUid uid, RottingComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
            return;
        RemCompDeferred(uid, component);
    }

    private void OnRejuvenate(EntityUid uid, RottingComponent component, RejuvenateEvent args)
    {
        RemCompDeferred<RottingComponent>(uid);
    }

    private void OnExamined(Entity<RottingComponent> rotting, ref ExaminedEvent args)
    {
        var examineText = GetRottingExamineText(rotting);
        args.PushMarkup(examineText);
    }

    [PublicAPI]
    public string GetPerishableExamineText(Entity<PerishableComponent> entity)
    {
        var stage = PerishStage(entity, MaxStages);
        if (stage < 1 || stage > MaxStages)
            return string.Empty;

        var suffix = stage.ToString();
        if (HasComp<MobStateComponent>(entity))
            suffix += "-nonmob";

        var description = "perishable-" + suffix;
        return Loc.GetString(description, ("target", Identity.Entity(entity, EntityManager)));
    }

    [PublicAPI]
    public string GetRottingExamineText(Entity<RottingComponent> entity)
    {
        var comp = entity.Comp;
        var stage = RotStage(entity, comp);
        var description = stage switch
        {
            >= 2 => "rotting-extremely-bloated",
            >= 1 => "rotting-bloated",
            _ => "rotting-rotting"
        };

        if (!HasComp<MobStateComponent>(entity))
            description += "-nonmob";

        return Loc.GetString(description, ("target", Identity.Entity(entity, EntityManager)));
    }

    /// <summary>
    /// Return an integer from 0 to maxStage representing how close to rotting an entity is. Used to
    /// generate examine messages for items that are starting to rot.
    /// </summary>
    public int PerishStage(Entity<PerishableComponent> perishable, int maxStages)
    {
        if (perishable.Comp.RotAfter.TotalSeconds == 0 || perishable.Comp.RotAccumulator.TotalSeconds == 0)
            return 0;
        return (int) (1 + maxStages * perishable.Comp.RotAccumulator.TotalSeconds / perishable.Comp.RotAfter.TotalSeconds);
    }

    public bool IsRotProgressing(EntityUid uid, PerishableComponent? perishable)
    {
        // things don't perish by default.
        if (!Resolve(uid, ref perishable, false))
            return false;

        // only dead things or inanimate objects can rot
        if (TryComp<MobStateComponent>(uid, out var mobState) && !_mobState.IsDead(uid, mobState))
            return false;

        if (HasComp<DragonUnrottingComponent>(uid))
            return false;

        if (_container.TryGetOuterContainer(uid, Transform(uid), out var container) &&
            HasComp<AntiRottingContainerComponent>(container.Owner))
        {
            return false;
        }

        if (TryComp<CuffableComponent>(uid, out var cuffed) && cuffed.CuffedHandCount > 0)
        {
            if (TryComp<HandcuffComponent>(cuffed.LastAddedCuffs, out var cuffcomp))
            {
                if (cuffcomp.NoRot)
                {
                    return false;
                }
            }
        }

        var ev = new IsRottingEvent();
        RaiseLocalEvent(uid, ref ev);

        return !ev.Handled;
    }

    public bool IsRotten(EntityUid uid, RottingComponent? rotting = null)
    {
        return Resolve(uid, ref rotting, false);
    }

    public void ReduceAccumulator(EntityUid uid, TimeSpan time)
    {
        if (!TryComp<PerishableComponent>(uid, out var perishable))
            return;

        if (!TryComp<RottingComponent>(uid, out var rotting))
        {
            perishable.RotAccumulator -= time;
            return;
        }
        var total = (rotting.TotalRotTime + perishable.RotAccumulator) - time;

        if (total < perishable.RotAfter)
        {
            RemCompDeferred(uid, rotting);
            perishable.RotAccumulator = total;
        }

        else
            rotting.TotalRotTime = total - perishable.RotAfter;
    }

    /// <summary>
    /// Transfers accumulated rot from one entity to another, scaling the time proportioanlly if needed.
    /// Does not transfer rotting level; use TransferRot for that.
    /// </summary>
    /// <param name="perishableFrom">The entity to transfer rot accumulation from.</param>
    /// <param name="perishableTo">The entity whose rot accumulation is being replaced.</param>
    /// <param name="proportional">Whether the rot accumulation on the receiving entity should be relative to its own expiration date.</param>
    /// <param name="butcherableFrom">Optional, ButcherableComponent on the "from" entity. The FreshnessIncrease field of the component is used to add a flat modifier to the freshness transfer time.</param>
    [PublicAPI]
    public void TransferFreshness(EntityUid fromId,
        PerishableComponent perishableFrom,
        PerishableComponent perishableTo,
        bool proportional = true,
        ButcherableComponent? butcherableFrom = null)
    {
        TimeSpan newRotAccumulator = perishableFrom.RotAccumulator;

        if (proportional)
        {
            var ratio = perishableFrom.RotAccumulator / perishableFrom.RotAfter;
            newRotAccumulator = perishableTo.RotAfter * ratio;
        }

        if (butcherableFrom != null && !HasComp<RottingComponent>(fromId))
            newRotAccumulator -= butcherableFrom.FreshnessIncrease;

        if (newRotAccumulator < TimeSpan.Zero)
            newRotAccumulator = TimeSpan.Zero;

        perishableTo.RotAccumulator = newRotAccumulator;
    }

    /// <summary>
    /// Transfers accumulated rot from one entity to another, scaling the time proportioanlly if needed.
    /// Does not transfer rotting level; use TransferRot for that.
    /// </summary>
    /// <param name="fromId">The entity to transfer rot accumulation from.</param>
    /// <param name="toId">The entity whose rot accumulation should be replaced.</param>
    /// <param name="proportional">Whether the rot accumulation on the receiving entity should be relative to its own expiration date.</param>
    /// <param name="butcherableFrom">Optional, ButcherableComponent on the "from" entity. The FreshnessIncrease field of the component is used to add a flat modifier to the freshness transfer time.</param>
    [PublicAPI]
    public void TransferFreshness(EntityUid fromId,
        EntityUid toId,
        bool proportional = true,
        ButcherableComponent? butcherableFrom = null)
    {
        if (!TryComp<PerishableComponent>(fromId, out var perishableFrom)
            || !TryComp<PerishableComponent>(toId, out var perishableTo))
            return;

        TransferFreshness(fromId, perishableFrom, perishableTo, proportional, butcherableFrom);
    }

    /// <summary>
    /// Transfers rotting amount from a rotten entity to a receiving entity.
    /// If the receiver is not already rotting, it will gain RottingComponent from this operation.
    /// </summary>
    /// <param name="rottingFrom">RottenComponent on the rotting entity.</param>
    /// <param name="perishableFrom">PerishableComponent on the rotting entity.</param>
    /// <param name="toId">The entity ID that will have its rot stage set.</param>
    /// <param name="perishableTo">PerishableComponent on the "to" entity.</param>
    /// <param name="proportional">Whether rot stage transferred should be proportional to the expiration time of the target entity.</param>
    [PublicAPI]
    public void TransferRotStage(RottingComponent rottingFrom,
        PerishableComponent perishableFrom,
        EntityUid toId,
        PerishableComponent? perishableTo,
        bool proportional = true)
    {
        var rottingTo = EnsureComp<RottingComponent>(toId);

        if (!proportional || !Resolve(toId, ref perishableTo, false))
        {
            rottingTo.TotalRotTime = rottingFrom.TotalRotTime;
            return;
        }

        var ratio = rottingFrom.TotalRotTime / perishableFrom.RotAfter;
        rottingTo.TotalRotTime = perishableTo.RotAfter * ratio;
    }


    /// <summary>
    /// Transfers rotting amount from a rotten entity to a receiving entity.
    /// If the receiver is not already rotting, it will gain RottingComponent from this operation.
    /// </summary>
    /// <param name="fromId">The entity ID that will transfer its rot stage.</param>
    /// <param name="toId">The entity ID that will have its rot stage set.</param>
    /// <param name="rottingFrom">RottenComponent on the rotting entity.</param>
    /// <param name="proportional">Whether rot stage transferred should be proportional to the expiration time of the target entity.</param>
    [PublicAPI]
    public void TransferRotStage(EntityUid fromId,
        EntityUid toId,
        RottingComponent rottingFrom,
        bool proportional = true)
    {
        if (!TryComp<PerishableComponent>(fromId, out var perishableFrom)
            || !TryComp<PerishableComponent>(toId, out var perishableTo))
            return;

        TransferRotStage(rottingFrom, perishableFrom, toId, perishableTo, proportional);
    }

    /// <summary>
    /// Transfers rotting amount from a rotten entity to a receiving entity.
    /// If the receiver is not already rotting, it will gain RottingComponent from this operation.
    /// </summary>
    /// <param name="fromId">The entity ID that will transfer its rot stage.</param>
    /// <param name="toId">The entity ID that will have its rot stage set.</param>
    /// <param name="proportional">Whether rot stage transferred should be proportional to the expiration time of the target entity.</param>
    [PublicAPI]
    public void TransferRotStage(EntityUid fromId, EntityUid toId, bool proportional = true)
    {
        if (!TryComp<RottingComponent>(fromId, out var rottingFrom))
            return;

        TransferRotStage(fromId, toId, rottingFrom, proportional);
    }

    /// <summary>
    /// Return the rot stage, usually from 0 to 2 inclusive.
    /// </summary>
    public int RotStage(EntityUid uid, RottingComponent? comp = null, PerishableComponent? perishable = null)
    {
        if (!Resolve(uid, ref comp, ref perishable))
            return 0;

        return (int) (comp.TotalRotTime.TotalSeconds / perishable.RotAfter.TotalSeconds);
    }
}
