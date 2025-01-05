using Content.Shared.Rejuvenate;

namespace Content.Server.FloofStation;

public sealed class AnomalyJobSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AnomalyJobComponent, ComponentStartup>(OnInit);
    }

    private void OnInit(EntityUid uid, AnomalyJobComponent component, ComponentStartup args)
    {
        RaiseLocalEvent(uid, new RejuvenateEvent());
    }
}
