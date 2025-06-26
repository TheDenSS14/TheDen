using Content.Shared._DEN.Body;
using Content.Shared._DEN.Movement.Components;
using Content.Shared.Hands;
using Content.Shared.Inventory;

namespace Content.Shared._DEN.Movement.Systems;

public abstract class SharedSupportStandingSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<SupportStandingComponent,
            InventoryRelayedEvent<CannotSupportStandingEvent>>(SupportStandingWhenWorn);
        SubscribeLocalEvent<SupportStandingComponent,
            HeldRelayedEvent<CannotSupportStandingEvent>>(SupportStandingInHands);
    }

    protected void SupportStandingInHands(Entity<SupportStandingComponent> ent,
        ref HeldRelayedEvent<CannotSupportStandingEvent> args)
    {
        if (args.Args.LegCount >= ent.Comp.MinimumLegCount && ent.Comp.WorksInHand)
            args.Args.Cancel();
    }

    protected void SupportStandingWhenWorn(Entity<SupportStandingComponent> ent,
        ref InventoryRelayedEvent<CannotSupportStandingEvent> args)
    {
        if (args.Args.LegCount >= ent.Comp.MinimumLegCount && ent.Comp.WorksWhenEquipped)
            args.Args.Cancel();
    }
}
