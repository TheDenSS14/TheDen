using Content.Server.Consent;
using Content.Shared._DEN.Devourable;
using Content.Shared.Consent;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;


namespace Content.Server._DEN.Devourable;


/// <summary>
/// This handles...
/// </summary>
public sealed class DevourableSystem : EntitySystem
{
    [Dependency] private readonly ConsentSystem _consentSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    private readonly ProtoId<ConsentTogglePrototype> _noDragonDevour = "NoDragonDevour";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawned);
        SubscribeLocalEvent<DevourableComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(Entity<DevourableComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Alive && ent.Comp.AttemptedDevouring)
        {
            ent.Comp.AttemptedDevouring = false;
            Dirty(ent);
        }
    }

    private void OnPlayerSpawned(PlayerSpawnCompleteEvent ev)
    {
        var devourable = EnsureComp<DevourableComponent>(ev.Mob);
        UpdateConsent((ev.Mob, devourable));
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<DevourableComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.LastUpdateTime + component.UpdateInterval > _gameTiming.CurTime)
                continue;

            component.LastUpdateTime = _gameTiming.CurTime;
            UpdateConsent((uid, component));
            Dirty(uid, component);
        }
    }

    private void UpdateConsent(Entity<DevourableComponent> ent)
    {
        // we're checking if they have it on (no devour)
        if (!TryComp<MindComponent>(ent, out var mindComponent) || mindComponent.UserId is not { })
            return;

        var dragonDevour = !_consentSystem.HasConsent(ent.Owner, _noDragonDevour);

        ent.Comp.IsDevourable = dragonDevour;
        Dirty(ent);
    }
}
