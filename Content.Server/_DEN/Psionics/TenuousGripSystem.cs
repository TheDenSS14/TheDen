using Content.Server.Abilities.Psionics;
using Content.Shared._DEN.Traits;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Mobs;
using Content.Shared.Psionics.Glimmer;


namespace Content.Server._DEN.Psionics;

public sealed class TenuousGripSystem : EntitySystem
{
    [Dependency] private readonly GlimmerSystem _glimmer = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionics = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TenuousGripComponent, MobStateChangedEvent>(OnDeath);
    }

    private void OnDeath(Entity<TenuousGripComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead ||
            !TryComp<PsionicComponent>(ent, out var psionic))
            return;

        TryComp<InnatePsionicPowersComponent>(ent, out var innatePowers);
        foreach (var power in psionic.ActivePowers)
        {
            if(!innatePowers?.PowersToAdd.Contains(power) ?? true)
                _psionics.RemovePsionicPower(ent, power);
        }
    }
}
