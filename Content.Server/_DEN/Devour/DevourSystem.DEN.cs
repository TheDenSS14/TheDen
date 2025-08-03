using Content.Shared._DEN.Unrotting;
using Content.Shared.Mobs;


namespace Content.Server.Devour;

public sealed partial class DevourSystem
{
    private void OnMobStateChanged(Entity<DragonUnrottingComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Alive)
            RemComp<DragonUnrottingComponent>(ent);
    }
}
