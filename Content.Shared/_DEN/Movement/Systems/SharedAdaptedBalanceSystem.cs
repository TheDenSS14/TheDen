// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Movement.Components;
using Content.Shared.Body.Components;
using Content.Shared.Buckle.Components;
using Content.Shared.Standing;
using Robust.Shared.Timing;

namespace Content.Shared._DEN.Movement.Systems;

public sealed class SharedAdaptedBalanceSystem : EntitySystem
{
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AdaptedBalanceComponent, ComponentInit>(OnAdaptedBalanceStartup);
        SubscribeLocalEvent<AdaptedBalanceComponent, StandingSupportLostEvent>(OnStandingSupportLost);
        SubscribeLocalEvent<AdaptedBalanceComponent, UnbuckledEvent>(OnUnbuckled);
        SubscribeLocalEvent<ActiveAdaptedBalanceComponent, CannotSupportStandingEvent>(TryStandWhenAdapted);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveAdaptedBalanceComponent>();
        var toUpdate = new List<EntityUid>();

        while (query.MoveNext(out var uid, out var balance))
        {
            if (_timing.CurTime > balance.EndTime)
                toUpdate.Add(uid);
        }

        foreach (var uid in toUpdate)
            _standing.UpdateStanding(uid);
    }

    private void OnAdaptedBalanceStartup(Entity<AdaptedBalanceComponent> ent, ref ComponentInit args)
        => AddBalance(ent);

    private void OnStandingSupportLost(Entity<AdaptedBalanceComponent> ent, ref StandingSupportLostEvent args)
        => AddBalance(ent);

    private void OnUnbuckled(Entity<AdaptedBalanceComponent> ent, ref UnbuckledEvent args)
        => AddBalance(ent);

    private void TryStandWhenAdapted(Entity<ActiveAdaptedBalanceComponent> ent,
        ref CannotSupportStandingEvent args)
    {
        if (_timing.CurTime > ent.Comp.EndTime)
        {
            RemComp(ent.Owner, ent.Comp);
            return;
        }

        args.Cancel();
    }

    private void AddBalance(Entity<AdaptedBalanceComponent> ent)
    {
        if (!TryComp<BodyComponent>(ent.Owner, out var body)
            || body.LegEntities.Count < ent.Comp.MinimumLegs)
            return;

        EnsureComp<ActiveAdaptedBalanceComponent>(ent.Owner, out var balance);
        balance.EndTime = _timing.CurTime + ent.Comp.BalanceDuration;
    }
}
